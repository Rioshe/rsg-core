namespace RSG
{
    public static class CoreServices
    {
        public static SplashSystem Splash { get; private set; }
        public static SceneSystem Scenes { get; private set; }
        public static TransitionSystem Transition { get; private set; }

        public static void Register(SplashSystem system) => Splash = system;
        public static void Register(SceneSystem system) => Scenes = system;
        public static void Register(TransitionSystem system) => Transition = system;
        
        
        public static void Clear()
        {
            Splash = null;
            Scenes = null;
            Transition = null;
        }

    }
}