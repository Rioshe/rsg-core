using UnityEngine;

namespace RSG
{
    [RequireComponent(typeof(Canvas))]
    public class ScreenCanvas : MonoBehaviour
    {
        private void Awake()
        {
            ScreenManager.Instance.RegisterCanvas(GetComponent<Canvas>());
        }
    }
}