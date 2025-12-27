using System;

namespace RSG
{
    public static class SceneEvents
    {
        public static event Action<string> OnRequestSceneLoad;
        public static void RaiseRequestSceneLoad(string sceneName) => OnRequestSceneLoad?.Invoke(sceneName);

        public static event Action<string> OnSceneLoadStarted;
        public static void RaiseSceneLoadStarted(string sceneName) => OnSceneLoadStarted?.Invoke(sceneName);

        public static event Action<string, float> OnSceneLoadProgress;
        public static void RaiseSceneLoadProgress(string sceneName, float progress) => OnSceneLoadProgress?.Invoke(sceneName, progress);

        public static event Action<string> OnSceneLoadComplete;
        public static void RaiseSceneLoadComplete(string sceneName) => OnSceneLoadComplete?.Invoke(sceneName);
    }
}