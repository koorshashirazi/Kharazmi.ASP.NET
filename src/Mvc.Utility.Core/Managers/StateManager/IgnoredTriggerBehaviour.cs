using System;

namespace Mvc.Utility.Core.Managers.StateManager
{
    public partial class StateMachine<TState, TTrigger>
    {
        internal class IgnoredTriggerBehaviour : TriggerBehaviour
        {
            public IgnoredTriggerBehaviour(TTrigger trigger, Func<bool> guard)
                : this(trigger, guard, string.Empty)
            {
            }

            public IgnoredTriggerBehaviour(TTrigger trigger, Func<bool> guard, string description)
                : base(trigger, guard, description)
            {
            }

            public override bool ResultsInTransitionFrom(TState source, object[] args, out TState destination)
            {
                destination = default;
                return false;
            }
        }
    }
}