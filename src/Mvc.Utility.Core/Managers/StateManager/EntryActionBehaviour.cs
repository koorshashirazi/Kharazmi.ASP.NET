using System;
using System.Threading.Tasks;

namespace Mvc.Utility.Core.Managers.StateManager
{
    public partial class StateMachine<TState, TTrigger>
    {
        internal abstract class EntryActionBehavior
        {
            protected EntryActionBehavior(string actionDescription)
            {
                ActionDescription = Enforce.ArgumentNotNull(actionDescription, nameof(actionDescription));
            }

            internal string ActionDescription { get; }

            public abstract void Execute(Transition transition, object[] args);
            public abstract Task ExecuteAsync(Transition transition, object[] args);

            public class Sync : EntryActionBehavior
            {
                private readonly Action<Transition, object[]> _action;

                public Sync(Action<Transition, object[]> action, string actionDescription) : base(actionDescription)
                {
                    _action = action;
                }

                public override void Execute(Transition transition, object[] args)
                {
                    _action(transition, args);
                }

                public override Task ExecuteAsync(Transition transition, object[] args)
                {
                    Execute(transition, args);
                    return TaskResult.Done;
                }
            }

            public class Async : EntryActionBehavior
            {
                private readonly Func<Transition, object[], Task> _action;

                public Async(Func<Transition, object[], Task> action, string actionDescription) : base(
                    actionDescription)
                {
                    _action = action;
                }

                public override void Execute(Transition transition, object[] args)
                {
                    throw new InvalidOperationException(
                        $"Cannot execute asynchronous action specified in OnEntry event for '{transition.Destination}' state. " +
                        "Use asynchronous version of Fire [FireAsync]");
                }

                public override Task ExecuteAsync(Transition transition, object[] args)
                {
                    return _action(transition, args);
                }
            }
        }
    }
}