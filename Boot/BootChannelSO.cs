using UnityEngine;
using UnityEngine.Events;

namespace RSG
{
    [CreateAssetMenu(menuName = "Channels/RSG/BootChannel", fileName = "SystemReadyChannel")]
    public class BootChannelSO : ScriptableObject
    {
        public UnityAction OnBootComplete;

        public void RaiseEvent()
        {
            OnBootComplete?.Invoke(  );
        }
    }
}