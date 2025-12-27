namespace RSG
{
    public static class UIServices
    {
        public static ScreenSystem Screens { get; private set; }

        public static void Register(ScreenSystem system) => Screens = system;
        
        
        public static void Clear()
        {
            Screens = null;
        }

    }
}

