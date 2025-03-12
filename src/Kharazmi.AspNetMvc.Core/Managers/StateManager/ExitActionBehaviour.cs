﻿using System;
using System.Threading.Tasks;

namespace Kharazmi.AspNetMvc.Core.Managers.StateManager
{
    public partial class StateMachine<TState, TTrigger>
    {
        internal abstract class ExitActionBehavior
        {
            protected ExitActionBehavior(string actionDescription)
            {
                ActionDescription = Enforce.ArgumentNotNull(actionDescription, nameof(actionDescription));
            }

            internal string ActionDescription { get; }

            public abstract void Execute(Transition transition);
            public abstract Task ExecuteAsync(Transition transition);

            public class Sync : ExitActionBehavior
            {
                private readonly Action<Transition> _action;

                public Sync(Action<Transition> action, string actionDescription) : base(actionDescription)
                {
                    _action = action;
                }

                public override void Execute(Transition transition)
                {
                    _action(transition);
                }

                public override Task ExecuteAsync(Transition transition)
                {
                    Execute(transition);
                    return TaskResult.Done;
                }
            }

            public class Async : ExitActionBehavior
            {
                private readonly Func<Transition, Task> _action;

                public Async(Func<Transition, Task> action, string actionDescription) : base(actionDescription)
                {
                    _action = action;
                }

                public override void Execute(Transition transition)
                {
                    throw new InvalidOperationException(
                        $"Cannot execute asynchronous action specified in OnExit event for '{transition.Source}' state. " +
                        "Use asynchronous version of Fire [FireAsync]");
                }

                public override Task ExecuteAsync(Transition transition)
                {
                    return _action(transition);
                }
            }
        }
    }
}