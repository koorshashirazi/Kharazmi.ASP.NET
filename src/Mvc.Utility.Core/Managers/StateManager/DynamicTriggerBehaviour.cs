﻿using System;

namespace Mvc.Utility.Core.Managers.StateManager
{
    public partial class StateMachine<TState, TTrigger>
    {
        internal class DynamicTriggerBehaviour : TriggerBehaviour
        {
            private readonly Func<object[], TState> _destination;

            public DynamicTriggerBehaviour(TTrigger trigger, Func<object[], TState> destination, Func<bool> guard
                , string description)
                : base(trigger, guard, description)
            {
                _destination = Enforce.ArgumentNotNull(destination, "destination");
            }

            public override bool ResultsInTransitionFrom(TState source, object[] args, out TState destination)
            {
                destination = _destination(args);
                return true;
            }
        }
    }
}