using System.Collections.Generic;

namespace RSG.HierarchicalStateMachine
{
    public class StateMachine
    {
        public readonly State RootState;
        public readonly TransitionSequencer Sequencer;
        private bool _started;

        public StateMachine( State rootState)
        {
            RootState = rootState;
            Sequencer = new TransitionSequencer(this);
        }

        public void Start()
        {
            if (_started)
                return;
            
            _started = true;
            RootState.Enter();
        }

        public void Tick( float deltaTime )
        {
            if( !_started )
            {
                Start();
            }
            InternalTick(deltaTime);
        }

        internal void InternalTick( float deltaTime )
        {
            RootState.Update( deltaTime );
        }

        public void ChangeState( State from, State to )
        {
            if( from == to || from == null || to == null )
                return;

            State lca = TransitionSequencer.LowestCommonAncestor( from, to );

            for( State s = from; s != lca; s = s.ParentState )
            {
                s.Exit();
            }
            
            Stack<State> stateStack = new Stack<State>();
            for( State s = to; s != lca; s = s.ParentState )
            {
                stateStack.Push( s );
            }

            while( stateStack.Count > 0 )
            {
                State s = stateStack.Pop();
                s.Enter();
            }
        }
    }
}