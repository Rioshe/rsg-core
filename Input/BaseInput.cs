using UnityEngine;
using UnityEngine.InputSystem;

namespace RSG.Core
{
    public abstract class BaseInput<T> : MonoBehaviour where T : IInputActionCollection2, new()
    {
        [Header("Input Configuration")]
        [Tooltip("The Scriptable Object that provides the Input Actions and global state.")]
        [SerializeField] private InputProviderSO<T> _inputProvider;

        protected T InputAction
        {
            get => _inputProvider.InputActions;
        }

        public T GetInputAction()
        {
            return InputAction;
        }

        private void Awake()
        {
            if (!_inputProvider)
            {
                Debug.LogError($"InputProviderSO not set on {gameObject.name}. Disabling component.", this);
                enabled = false;
            }
        }

        private void OnEnable()
        {
            if (!_inputProvider) return;
            _inputProvider.RegisterListener();
            SubscribeToActionEvents();
        }

        private void OnDisable()
        {
            if (!_inputProvider) return;
            _inputProvider.UnregisterListener();
            UnsubscribeToActionEvents();
        }

        protected abstract void SubscribeToActionEvents();
        protected abstract void UnsubscribeToActionEvents();
    }
}