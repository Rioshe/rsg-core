using UnityEngine;

namespace Core
{
    public class KillMePlease : MonoBehaviour
    {
        [SerializeField] private bool m_onAwake = true;
        [SerializeField] private float m_killTime;

        private void Awake()
        {
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