using UnityEngine;
using UnityEngine.Events;

namespace RSG
{
    [CreateAssetMenu(menuName = "Channels/RSG/BootChannel", fileName = "SystemReadyChannel")]
    public class BootChannelSO : ScriptableObject
    {
        public UnityAction<int, int> OnBootProgressEvent;
        public void RaiseBootProgressEvent( int progress, int total )
        {
            OnBootProgressEvent?.Invoke( progress, total );
        }
        
        public UnityAction OnBootCompleteEvent;
        public void RaiseBootCompleteEvent()
        {
            OnBootCompleteEvent?.Invoke();
        }
    }
}