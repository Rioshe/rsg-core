using UnityEngine.InputSystem;

namespace RSG.Input
{
    public static class InputSystemEvents<T> where T : IInputActionCollection2
    {
        public static event System.Action EnableAllInputs;
        public static event System.Action DisableAllInputs;
        
        public static event System.Action<T> EnableInput;
        public static event System.Action<T> DisableInput;
        
        
        public static void SendEnableAllInputs()
        {
            EnableAllInputs?.Invoke();
        }
        
        public static void SendDisableAllInputs()
        {
            DisableAllInputs?.Invoke();
        }
        
        public static void SendEnableInput(T inputActionCollection2 = default)
        {
            EnableInput?.Invoke(inputActionCollection2);
        }
        
        public static void SendDisableInput(T inputActionCollection2 = default)
        {
            DisableInput?.Invoke(inputActionCollection2);
        }
    }
}