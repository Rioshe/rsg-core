using System;

namespace RSG.FiniteStateMachine
{
    public class StateMachine
    {
        public int CurrentState
        {
            get => m_currentState;
        }

        public int LastState
        {
            get => m_lastState;
        }

        private int m_currentState = -1;
        private int m_targetState = -1;
        private int m_lastState = -1;

        private readonly IState[] m_states;
        private readonly Action<int> m_debugStateChangeCallback;
        
        public StateMachine(int numStates, Action<int> debugStateChangeCallback = null)
        {
            m_states = new IState[numStates];
            m_debugStateChangeCallback = debugStateChangeCallback;
        }
     
        public void RegisterState(Enum stateEnum, IState state)
        {
            int stateInt = Convert.ToInt32(stateEnum);
            m_states[stateInt] = state;
            state.OnInitialize(this); 
        }

        public void SetState(Enum targetState)
        {
            int targetStateInt = Convert.ToInt32(targetState);
            SetState(targetStateInt);
        }
        
        public void SetState(int targetState)
        {
            if(targetState == m_currentState)
                return;
        
            if (targetState >= 0)
            {
                m_targetState = targetState;

#if PROJECT_DEBUG
                m_debugStateChangeCallback?.Invoke(m_targetState);
#endif
            }
        }

        public bool IsState(Enum stateEnum)
        {
            int stateInt = Convert.ToInt32(stateEnum); 
            return m_currentState == stateInt;
        }

        public IState GetState(Enum stateEnum)
        {
            int stateInt = Convert.ToInt32(stateEnum); 
            return m_states[stateInt];
        }

        public void Update()
        {
            if (m_currentState != m_targetState)
            {
                if (m_currentState >= 0)
                {
                    m_states[m_currentState].OnExit();
                }
            
                m_lastState = m_currentState;
                m_currentState = m_targetState;
                m_states[m_targetState].OnEnter();
            }

            if (m_currentState >= 0)
            {
                m_states[CurrentState].OnUpdate();
            }
        }

        public void FixedUpdate()
        {
            if (CurrentState >= 0)
            {
                m_states[CurrentState].OnFixedUpdate();
            }
        }
        
    }
}