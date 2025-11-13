using System.Collections.Generic;
using System.Reflection;

namespace RSG.HierarchicalStateMachine
{
    public class StateMachineBuilder
    {
        readonly State rootState;

        public StateMachineBuilder( State rootState )
        {
            this.rootState = rootState;
        }

        void Wire( State state, StateMachine stateMachine, HashSet<State> visitedStates )
        {
            if(state == null)return;
            if(!visitedStates.Add(state)) return;
            
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
            FieldInfo machineField = typeof(State).GetField("Machine", flags);
            if( machineField != null )
            {
                machineField.SetValue( state, stateMachine );
            }

            foreach( FieldInfo fieldInfo in state.GetType().GetFields(flags) )
            {
                if(!typeof(State).IsAssignableFrom(fieldInfo.FieldType)) continue;
                if( fieldInfo.Name == "ParentState" ) continue;
                
                State fieldOfTypeState = (State) fieldInfo.GetValue( state );
                if(fieldOfTypeState == null) continue;
                
                if(!ReferenceEquals( fieldOfTypeState.ParentState, state )) continue;
                Wire( fieldOfTypeState, stateMachine, visitedStates );
            }
        }

        public StateMachine Build()
        {
            StateMachine newStateMachine = new StateMachine( rootState );
            Wire( rootState, newStateMachine, new HashSet<State>());
            return newStateMachine;
        }
    }
}