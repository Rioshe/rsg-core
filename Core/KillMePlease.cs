using UnityEngine;

namespace RSG.Core
{
    public class KillMePlease : MonoBehaviour
    {
        [SerializeField] private bool m_onAwake = true;
        [SerializeField] private float m_killTime;

        private void Awake()
        {
#if !PROJECT_DEBUG
            Destroy(gameObject);
            return;
#endif
            
            if (m_onAwake)
                Destroy(gameObject, m_killTime);
        }
        private void Start()
        {
            if (!m_onAwake)
                Destroy(gameObject, m_killTime);
        }
    }
}