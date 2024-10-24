namespace CrossEngine.System.FSM
{
    public abstract class FSMExample<TFSM, TState> where TFSM : FinalStateMachine<TState>, new() where TState : IState
    {
        protected TFSM _stateMachine = new();

        protected void Update()
        {
            if (_stateMachine.CurrentState is null) throw new CrossException("Current state is null.");
            _stateMachine.CurrentState.Update();
        }

        public TState GetState<T>() where T : TState
        {
            return _stateMachine.GetState<T>();
        }
    }
}
