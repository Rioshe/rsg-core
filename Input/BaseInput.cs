using UnityEngine;
using UnityEngine.InputSystem;

namespace RSG.Input
{
    public abstract class BaseInput<T> : MonoBehaviour where T : IInputActionCollection2, new()
    {
        protected T InputAction;
        private bool _isComponentEnabled = false;

        public T GetInputAction() => InputAction;

        private void Awake()
        {
            InputAction = new T();
        }

        private void OnEnable()
        {
            _isComponentEnabled = true;
            Subscribe();
            UpdateInputState();
        }

        private void OnDisable()
        {
            _isComponentEnabled = false;
            Unsubscribe();
            DisableInput();
        }

        private void Subscribe()
        {
            InputEvents<T>.OnStateChanged += OnGlobalStateChanged;
            SubscribeToActionEvents();
        }

        private void Unsubscribe()
        {
            InputEvents<T>.OnStateChanged -= OnGlobalStateChanged;
            UnsubscribeToActionEvents();
        }

        private void OnGlobalStateChanged(bool isGloballyEnabled)
        {
            UpdateInputState();
        }

        private void UpdateInputState()
        {
            if (_isComponentEnabled && InputEvents<T>.IsGloballyEnabled)
            {
                EnableInput();
            }
            else
            {
                DisableInput();
            }
        }

        protected void EnableInput()
        {
            InputAction?.Enable();
        }

        protected void DisableInput()
        {
            InputAction?.Disable();
        }

        protected abstract void SubscribeToActionEvents();
        protected abstract void UnsubscribeToActionEvents();
    }
}