using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public abstract class BaseInput<T> : MonoBehaviour where T : IInputActionCollection2
    {
        protected T InputAction;
        
        private void Awake()
        {
            InputAction = GetInput();
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
            InputSystemEvents<T>.EnableInput += OnEnableInput;
            InputSystemEvents<T>.DisableInput += OnDisableInput;
            
            InputSystemEvents<T>.EnableAllInputs += EnableInput;
            InputSystemEvents<T>.DisableAllInputs += DisableInput;
            
            SubscribeToEvents();
        }

        private void Unsubscribe()
        {
            InputSystemEvents<T>.EnableInput -= OnEnableInput;
            InputSystemEvents<T>.DisableInput -= OnDisableInput;
            
            InputSystemEvents<T>.EnableAllInputs -= EnableInput;
            InputSystemEvents<T>.DisableAllInputs -= DisableInput;
            
            UnsubscribeToEvents();
        }

        private void OnEnableInput(T input)
        {
            EnableInput();
        }

        private void OnDisableInput(T input)
        {
            DisableInput();
        }

        private void EnableInput()
        {
            InputAction?.Enable();
        }
        
        private void DisableInput()
        {
            InputAction?.Disable();
        }
        
        protected abstract T GetInput();
        protected abstract void SubscribeToEvents();
        protected abstract void UnsubscribeToEvents();
    }
}