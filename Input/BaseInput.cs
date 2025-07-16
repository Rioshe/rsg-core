using UnityEngine;
using UnityEngine.InputSystem;

namespace RSG.Input
{
    public abstract class BaseInput<T> : MonoBehaviour where T : IInputActionCollection2, new()
    {
        protected T InputAction;
        private bool _inputEnabled;

        public T GetInputAction() => InputAction;
        
        private void Awake()
        {
            InputAction = new T();
            EnableInput();
            Subscribe();
        }

        private void OnDestroy()
        {
            Unsubscribe();
            DisableInput();
        }

        private void Subscribe()
        {
            InputEvents<T>.EnableInput += OnEnableInput;
            InputEvents<T>.DisableInput += OnDisableInput;
            InputEvents<T>.EnableAllInputs += EnableInput;
            InputEvents<T>.DisableAllInputs += DisableInput;

            SubscribeToActionEvents();
        }

        private void Unsubscribe()
        {
            InputEvents<T>.EnableInput -= OnEnableInput;
            InputEvents<T>.DisableInput -= OnDisableInput;
            InputEvents<T>.EnableAllInputs -= EnableInput;
            InputEvents<T>.DisableAllInputs -= DisableInput;

            UnsubscribeToActionEvents();
        }

        private void OnEnableInput(T input)
        {
            EnableInput();
        }

        private void OnDisableInput(T input)
        {
            DisableInput();
        }

        protected void EnableInput()
        {
            if (_inputEnabled || InputAction == null) return;
            InputAction.Enable();
            _inputEnabled = true;
        }

        protected void DisableInput()
        {
            if (!_inputEnabled || InputAction == null) return;
            InputAction.Disable();
            _inputEnabled = false;
        }
        
        protected abstract void SubscribeToActionEvents();
        protected abstract void UnsubscribeToActionEvents();
    }
}
