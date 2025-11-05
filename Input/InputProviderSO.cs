using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace RSG.Core
{
    public abstract class InputProviderSO<T> : ScriptableObject where T : IInputActionCollection2, new()
    {
        [Tooltip("The Input Action instance managed by this Scriptable Object.")]
        public T InputActions { get; private set; }

        [Header("Global State Management")]
        [Tooltip("The default state for this input on game start.")]
        [SerializeField] private bool _defaultState = true;
        
        public bool IsGloballyEnabled { get; private set; }
        public event Action<bool> OnStateChanged;

        private int _activeListenersCount = 0;

        private void OnEnable()
        {
            InputActions ??= new T();
            IsGloballyEnabled = _defaultState;
            _activeListenersCount = 0;
        }

        public void RaiseEnable()
        {
            if (IsGloballyEnabled) return;
            IsGloballyEnabled = true;
            OnStateChanged?.Invoke(true);
            UpdateInputActionsState();
        }

        public void RaiseDisable()
        {
            if (!IsGloballyEnabled) return;
            IsGloballyEnabled = false;
            OnStateChanged?.Invoke(false);
            UpdateInputActionsState();
        }
        
        public void RegisterListener()
        {
            _activeListenersCount++;
            UpdateInputActionsState();
        }

        public void UnregisterListener()
        {
            _activeListenersCount--;
            UpdateInputActionsState();
        }
        
        private void UpdateInputActionsState()
        {
            if (IsGloballyEnabled && _activeListenersCount > 0)
            {
                InputActions?.Enable();
            }
            else
            {
                InputActions?.Disable();
            }
        }

    }
}