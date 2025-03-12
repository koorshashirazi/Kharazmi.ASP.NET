namespace Mvc.Utility.Core.Managers.StateManager
{
    public partial class StateMachine<TState, TTrigger>
    {
        internal class StateReference
        {
            public TState State { get; set; }
        }
    }
}