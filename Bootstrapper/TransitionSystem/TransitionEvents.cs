using System;

namespace RSG
{
    public static class TransitionEvents
    {
        public static event Action OnLoadingStarted;
        public static void RaiseLoadingStarted() => OnLoadingStarted?.Invoke();

        public static event Action OnLoadingComplete;
        public static void RaiseLoadingComplete() => OnLoadingComplete?.Invoke();

        public static event Action OnTransitionFadeIn;
        public static void RaiseTransitionFadeIn() => OnTransitionFadeIn?.Invoke();

        public static event Action OnTransitionFadeOut;
        public static void RaiseTransitionFadeOut() => OnTransitionFadeOut?.Invoke();
    }
}