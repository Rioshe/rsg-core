using System;

public static class ScreenEvents
{
    public static event Action<string> OnShowScreen;
    public static void RaiseShowScreen(string screenName) => OnShowScreen?.Invoke(screenName);
}

