using System.Collections.Generic;

namespace RSG.HierarchicalStateMachine
{
    public class TransitionSequencer
    {
        public readonly StateMachine Machine;

        public TransitionSequencer( StateMachine machine )
        {
            Machine = machine;
        }
        
        public void RequestTransition(State from, State to){}

        public static State LowestCommonAncestor( State a, State b )
        {
            HashSet<State> states = new HashSet<State>();
            for( State s = a; s != null; s = s.ParentState)
            {
                states.Add(s);
            }

            for( State s = b; s != null; s = s.ParentState )
            {
                if( states.Contains( s ) )
                {
                    return s;
                }
            }

            return null;
        }
    }
}