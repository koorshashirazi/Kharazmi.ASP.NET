using System;
using System.Collections.Generic;
using System.Linq;
using Mvc.Utility.Core.Managers.StateManager;

namespace Kharazmi.AspNetMvc.Core.Managers.StateManager
{
    public partial class StateMachine<TState, TTrigger>
    {
        internal class StateRepresentation
        {
            private readonly ICollection<ActivateActionBehaviour>
                _activateActions = new List<ActivateActionBehaviour>();

            private readonly ICollection<DeactivateActionBehaviour> _deactivateActions =
                new List<DeactivateActionBehaviour>();

            private readonly ICollection<InternalActionBehaviour>
                _internalActions = new List<InternalActionBehaviour>();

            private readonly ICollection<StateRepresentation> _subStates = new List<StateRepresentation>();

            private bool _active;

            public StateRepresentation(TState state)
            {
                UnderlyingState = state;
            }

            internal IDictionary<TTrigger, ICollection<TriggerBehaviour>> TriggerBehaviours { get; } =
                new Dictionary<TTrigger, ICollection<TriggerBehaviour>>();

            internal ICollection<EntryActionBehavior> EntryActions { get; } = new List<EntryActionBehavior>();
            internal ICollection<ExitActionBehavior> ExitActions { get; } = new List<ExitActionBehavior>();

            public StateRepresentation Superstate { get; set; }

            public TState UnderlyingState { get; }

            public IEnumerable<TTrigger> PermittedTriggers
            {
                get
                {
                    var result = TriggerBehaviours
                        .Where(t => t.Value.Any(a => a.IsGuardConditionMet))
                        .Select(t => t.Key);

                    if (Superstate != null) result = result.Union(Superstate.PermittedTriggers);

                    return result.ToArray();
                }
            }

            public bool CanHandle(TTrigger trigger)
            {
                TriggerBehaviour unused;
                return TryFindHandler(trigger, out unused);
            }

            public bool TryFindHandler(TTrigger trigger, out TriggerBehaviour handler)
            {
                return TryFindLocalHandler(trigger, out handler, t => t.IsGuardConditionMet) ||
                       Superstate != null && Superstate.TryFindHandler(trigger, out handler);
            }

            private bool TryFindLocalHandler(TTrigger trigger, out TriggerBehaviour handler
                , params Func<TriggerBehaviour, bool>[] filters)
            {
                ICollection<TriggerBehaviour> possible;
                if (!TriggerBehaviours.TryGetValue(trigger, out possible))
                {
                    handler = null;
                    return false;
                }

                var actual = filters.Aggregate(possible, (current, filter) => current.Where(filter).ToArray());

                if (actual.Count() > 1)
                    throw new InvalidOperationException(string.Format(
                        StateRepresentationResources.MultipleTransitionsPermitted, trigger, UnderlyingState));

                handler = actual.FirstOrDefault();
                return handler != null;
            }

            public bool TryFindHandlerWithUnmetGuardCondition(TTrigger trigger, out TriggerBehaviour handler)
            {
                return TryFindLocalHandler(trigger, out handler, t => !t.IsGuardConditionMet) || Superstate != null &&
                       Superstate.TryFindHandlerWithUnmetGuardCondition(trigger, out handler);
            }

            public void AddActivateAction(Action action, string activateActionDescription)
            {
                _activateActions.Add(new ActivateActionBehaviour.Sync(UnderlyingState,
                    Enforce.ArgumentNotNull(action, nameof(action)),
                    Enforce.ArgumentNotNull(activateActionDescription, nameof(activateActionDescription))));
            }

            public void AddDeactivateAction(Action action, string deactivateActionDescription)
            {
                _deactivateActions.Add(new DeactivateActionBehaviour.Sync(UnderlyingState,
                    Enforce.ArgumentNotNull(action, nameof(action)),
                    Enforce.ArgumentNotNull(deactivateActionDescription, nameof(deactivateActionDescription))));
            }

            public void AddEntryAction(TTrigger trigger, Action<Transition, object[]> action
                , string entryActionDescription)
            {
                Enforce.ArgumentNotNull(action, nameof(action));
                EntryActions.Add(
                    new EntryActionBehavior.Sync(
                        (t, args) =>
                        {
                            if (t.Trigger.Equals(trigger)) action(t, args);
                        },
                        Enforce.ArgumentNotNull(entryActionDescription, nameof(entryActionDescription))));
            }

            public void AddEntryAction(Action<Transition, object[]> action, string entryActionDescription)
            {
                EntryActions.Add(new EntryActionBehavior.Sync(Enforce.ArgumentNotNull(action, nameof(action)),
                    Enforce.ArgumentNotNull(entryActionDescription, nameof(entryActionDescription))));
            }

            public void AddExitAction(Action<Transition> action, string exitActionDescription)
            {
                ExitActions.Add(new ExitActionBehavior.Sync(Enforce.ArgumentNotNull(action, nameof(action)),
                    Enforce.ArgumentNotNull(exitActionDescription, nameof(exitActionDescription))));
            }

            internal void AddInternalAction(TTrigger trigger, Action<Transition, object[]> action)
            {
                Enforce.ArgumentNotNull(action, "action");

                _internalActions.Add(
                    new InternalActionBehaviour.Sync(
                        (t, args) =>
                        {
                            if (t.Trigger.Equals(trigger)) action(t, args);
                        }));
            }

            public void Activate()
            {
                if (Superstate != null) Superstate.Activate();

                if (_active) return;

                ExecuteActivationActions();
                _active = true;
            }

            public void Deactivate()
            {
                if (!_active) return;

                ExecuteDeactivationActions();
                _active = false;

                if (Superstate != null) Superstate.Deactivate();
            }

            private void ExecuteActivationActions()
            {
                foreach (var action in _activateActions) action.Execute();
            }

            private void ExecuteDeactivationActions()
            {
                foreach (var action in _deactivateActions) action.Execute();
            }

            public void Enter(Transition transition, params object[] entryArgs)
            {
                Enforce.ArgumentNotNull(transition, nameof(transition));

                if (transition.IsReentry)
                {
                    ExecuteEntryActions(transition, entryArgs);
                    ExecuteActivationActions();
                }
                else if (!Includes(transition.Source))
                {
                    if (Superstate != null) Superstate.Enter(transition, entryArgs);

                    ExecuteEntryActions(transition, entryArgs);
                    ExecuteActivationActions();
                }
            }

            public void Exit(Transition transition)
            {
                Enforce.ArgumentNotNull(transition, nameof(transition));

                if (transition.IsReentry)
                {
                    ExecuteDeactivationActions();
                    ExecuteExitActions(transition);
                }
                else if (!Includes(transition.Destination))
                {
                    ExecuteDeactivationActions();
                    ExecuteExitActions(transition);

                    if (Superstate != null) Superstate.Exit(transition);
                }
            }

            private void ExecuteEntryActions(Transition transition, object[] entryArgs)
            {
                Enforce.ArgumentNotNull(transition, nameof(transition));
                Enforce.ArgumentNotNull(entryArgs, nameof(entryArgs));
                foreach (var action in EntryActions) action.Execute(transition, entryArgs);
            }

            private void ExecuteExitActions(Transition transition)
            {
                Enforce.ArgumentNotNull(transition, nameof(transition));
                foreach (var action in ExitActions) action.Execute(transition);
            }

            private void ExecuteInternalActions(Transition transition, object[] args)
            {
                var possibleActions = new List<InternalActionBehaviour>();

                // Look for actions in superstate(s) recursivly until we hit the topmost superstate
                var aStateRep = this;
                do
                {
                    possibleActions.AddRange(aStateRep._internalActions);
                    aStateRep = aStateRep.Superstate;
                } while (aStateRep != null);

                // Execute internal transition event handler
                foreach (var action in possibleActions) action.Execute(transition, args);
            }

            public void AddTriggerBehaviour(TriggerBehaviour triggerBehaviour)
            {
                ICollection<TriggerBehaviour> allowed;
                if (!TriggerBehaviours.TryGetValue(triggerBehaviour.Trigger, out allowed))
                {
                    allowed = new List<TriggerBehaviour>();
                    TriggerBehaviours.Add(triggerBehaviour.Trigger, allowed);
                }

                allowed.Add(triggerBehaviour);
            }

            public void AddSubstate(StateRepresentation substate)
            {
                Enforce.ArgumentNotNull(substate, nameof(substate));
                _subStates.Add(substate);
            }

            public bool Includes(TState state)
            {
                return UnderlyingState.Equals(state) || _subStates.Any(s => s.Includes(state));
            }

            public bool IsIncludedIn(TState state)
            {
                return
                    UnderlyingState.Equals(state) || Superstate != null && Superstate.IsIncludedIn(state);
            }

            internal void InternalAction(Transition transition, object[] args)
            {
                Enforce.ArgumentNotNull(transition, "transition");
                ExecuteInternalActions(transition, args);
            }
        }
    }
}