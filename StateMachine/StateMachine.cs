using System;

namespace RSG
{
    public class StateMachine
    {
        private int m_currentState = -1;
        private int m_targetState = -1;
        private int m_lastState = -1;

        private readonly IState[] m_states;
        private readonly Action<int> m_debugStateChangeCallback;
        
        public StateMachine(Enum stateCount, Action<int> debugStateChangeCallback = null)
        {
            int count = Convert.ToInt32(stateCount);
            m_states = new IState[count];
            m_debugStateChangeCallback = debugStateChangeCallback;
        }
        
        public void RegisterState(Enum stateEnum, IState state)
        {
            int stateInt = Convert.ToInt32(stateEnum); 
            m_states[stateInt] = state;
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

        public int GetLastState()
        {
            return m_lastState;
        }

        public int GetCurrentState()
        {
            return m_currentState;
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
                m_states[m_currentState].OnUpdate();
            }
        }

        public void FixUpdate()
        {
            if (m_currentState >= 0)
            {
                m_states[m_currentState].OnFixedUpdate();
            }
        }
        
    }
}