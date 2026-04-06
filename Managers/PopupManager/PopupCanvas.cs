using UnityEngine;

namespace RSG
{
    [RequireComponent(typeof(Canvas))]
    public class PopupCanvas : MonoBehaviour
    {
        private void Awake()
        {
            PopupManager.Instance.RegisterCanvas(GetComponent<Canvas>());
        }
    }
}

