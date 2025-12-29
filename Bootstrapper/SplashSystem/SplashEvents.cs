using System;

namespace RSG
{
    public static class SplashEvents
    {
        public static event Action OnSplashStarted;
        public static void RaiseSplashStarted() => OnSplashStarted?.Invoke();

        public static event Action OnSplashSequenceComplete;
        public static void RaiseSplashSequenceComplete() => OnSplashSequenceComplete?.Invoke();

        public static event Action OnFadeOutStarted;
        public static void RaiseFadeOutStarted() => OnFadeOutStarted?.Invoke();

        public static event Action OnFadeOutComplete;
        public static void RaiseFadeOutComplete() => OnFadeOutComplete?.Invoke();
    }
}