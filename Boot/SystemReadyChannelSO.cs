using UnityEngine;
using UnityEngine.Events;

namespace RSG
{
    [CreateAssetMenu(menuName = "Channels/RSG/SystemReadyChannel", fileName = "SystemReadyChannel")]
    public class SystemReadyChannelSO : ScriptableObject
    {
        public UnityAction OnSystemsReady;

        public void RaiseEvent()
        {
            OnSystemsReady?.Invoke(  );
        }
    }
}