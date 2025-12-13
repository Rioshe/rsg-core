using UnityEngine;
using UnityEngine.Events;

namespace RSG
{
    [CreateAssetMenu(menuName = "RSG/Channels/LevelLoadChannel")]
    public class SystemReadyChannelSO : ScriptableObject
    {
        public UnityAction OnSystemsReady;

        public void RaiseEvent()
        {
            OnSystemsReady?.Invoke(  );
        }
    }
}