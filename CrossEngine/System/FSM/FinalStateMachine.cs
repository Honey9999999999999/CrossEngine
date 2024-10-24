namespace CrossEngine.System.FSM
{
    public abstract class FinalStateMachine<TState> where TState : IState
    {
        public TState? CurrentState { get; private set; }
        private Dictionary<Type, TState> _states;

        public FinalStateMachine()
        {
            _states = [];
        }        

        public void EnterIn<T>() where T : TState
        {
            var type = typeof(T);

            if (type != CurrentState?.GetType() && _states.TryGetValue(type, out TState state))
            {
                CurrentState?.Exit();
                CurrentState = state;
                CurrentState.Enter();
            }
        }
        public TState GetState<T>() where T : TState
        {
            return (T)_states[typeof(T)];
        }

        public void Update()
        {
            if (CurrentState is null) throw new CrossException("Current state is null.");
            CurrentState.Update();
        }

        public void AddState(TState state)
        {
            var type = state.GetType();
            if (!_states.TryGetValue(type, out _))
            {
                _states.Add(type, state);
            }
        }

        public void ClearStates()
        {
            _states.Clear();
        }

        public Type[] GetKeys() => [.. _states.Keys];
    }
}
