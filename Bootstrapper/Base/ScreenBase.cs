using UnityEngine;

namespace RSG
{
    public abstract class ScreenBase : MonoBehaviour
    { 
        public string ScreenId => GetType().Name;

        public virtual void OnShow() => gameObject.SetActive(true);
        public virtual void OnHide() => gameObject.SetActive(false);
    }
}