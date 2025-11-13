using System.Collections.Generic;

namespace RSG.HierarchicalStateMachine
{
    public abstract class State
    {
        public readonly StateMachine Machine;
        public readonly State ParentState;
        public State ActiveChildState;

        public State( StateMachine machine, State parentState )
        {
            Machine = machine;
            ParentState = parentState;
        }

        protected virtual State GetInitialState() => null;
        protected virtual State GetTransition() => null;
        
        
        internal void Enter()
        {
            if( ParentState != null )
                ParentState.ActiveChildState = this;
            
            OnEnter();
            
            State initialState = GetInitialState();
            if( initialState != null )
                initialState.Enter();
        }
        protected virtual void OnEnter(){}

        
        internal void Exit()
        {
            if(ActiveChildState != null)
                ActiveChildState.Exit();
            ActiveChildState =  null;
            OnExit();
        }
        protected virtual void OnExit(){}

        
        internal void Update( float deltaTime )
        {
            State transition = GetTransition();
            if( transition != null )
            {
                Machine.Sequencer.RequestTransition( this, transition );
                return;
            }
            
            if(ActiveChildState != null)
                ActiveChildState.Update( deltaTime );
            
            OnUpdate( deltaTime );
        }
        protected virtual void OnUpdate(float deltaTime) {}


        public State ActiveLeafState()
        {
            State leafState = this;
            while(leafState.ActiveChildState != null)
            {
                leafState = leafState.ActiveChildState;
            }
            
            return leafState;
        }

        public IEnumerable<State> PathToRootState()
        {
            for( State s = this; s != null; s = s.ParentState )
            {
                yield return s;
            }
        }
    }
}