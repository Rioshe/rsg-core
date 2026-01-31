using System;

namespace RSG
{
    public static class CoreUIEvents
    {
        public static event Action OnBackButtonPressed;
        public static event Action<int> OnRequestShowScreen;
        public static event Action OnRequestHideCurrent;

        public static event Action<int> OnScreenShow;
        public static event Action<int> OnScreenHide;

        public static void SendBackButtonPressed() => OnBackButtonPressed?.Invoke();
        public static void RequestShowScreen(int screenId) => OnRequestShowScreen?.Invoke(screenId);
        public static void RequestHideCurrent() => OnRequestHideCurrent?.Invoke();
        public static void SendScreenShow(int screenId) => OnScreenShow?.Invoke(screenId);
        public static void SendScreenHide(int screenId) => OnScreenHide?.Invoke(screenId);
    }
}