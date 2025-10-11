using UnityEngine.InputSystem;

namespace RSG.Input
{
    public static class InputEvents<T> where T : IInputActionCollection2
    {
        public static event System.Action EnableAllInputs;
        public static event System.Action DisableAllInputs;

        public static event System.Action<T> EnableInput;
        public static event System.Action<T> DisableInput;

        public static void RaiseEnableAllInputs()
        {
            EnableAllInputs?.Invoke();
        }

        public static void RaiseDisableAllInputs()
        {
            DisableAllInputs?.Invoke();
        }

        public static void RaiseEnableInput(T input = default)
        {
            EnableInput?.Invoke(input);
        }

        public static void RaiseDisableInput(T input = default)
        {
            DisableInput?.Invoke(input);
        }
    }
}
