using System;

namespace Kharazmi.AspNetMvc.Core.Managers.StateManager
{
    public partial class StateMachine<TState, TTrigger>
    {
        internal class TransitioningTriggerBehaviour : TriggerBehaviour
        {
            public TransitioningTriggerBehaviour(TTrigger trigger, TState destination, Func<bool> guard)
                : this(trigger, destination, guard, string.Empty)
            {
            }

            public TransitioningTriggerBehaviour(TTrigger trigger, TState destination, Func<bool> guard
                , string description)
                : base(trigger, guard, description)
            {
                Destination = destination;
            }

            internal TState Destination { get; }

            public override bool ResultsInTransitionFrom(TState source, object[] args, out TState destination)
            {
                destination = Destination;
                return true;
            }
        }
    }
}