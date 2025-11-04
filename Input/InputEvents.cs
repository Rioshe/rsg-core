using UnityEngine.InputSystem;
using System;

namespace RSG.Input
{
    public static class InputEvents<T> where T : IInputActionCollection2
    {
        public static bool IsGloballyEnabled { get; private set; } = true;
        
        public static event Action<bool> OnStateChanged;

        public static void RaiseEnable()
        {
            if (IsGloballyEnabled) return;
            IsGloballyEnabled = true;
            OnStateChanged?.Invoke(true);
        }

        public static void RaiseDisable()
        {
            if (!IsGloballyEnabled) return;
            IsGloballyEnabled = false;
            OnStateChanged?.Invoke(false);
        }
    }
}