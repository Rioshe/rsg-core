namespace RSG.FiniteStateMachine
{
    public interface IState
    {
        public void OnInitialize(StateMachine machine);
        public void OnEnter();
        public void OnUpdate();
        public void OnFixedUpdate();
        public void OnExit();
    }
}