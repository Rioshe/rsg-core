using UnityEngine;
using Object = UnityEngine.Object;

namespace RSG.Debugger
{
    public static class DebugLogger
    {
        public static void Log(string message, Object context = null, Color? color = null)
        {
#if PROJECT_DEBUG
                string colorHex = ColorUtility.ToHtmlStringRGB(color ?? Color.white);
                Debug.Log($"<color=#{colorHex}>{message}</color>", context);
#endif
        }

        public static void LogWarning(string message, Object context = null, Color? color = null)
        {
#if PROJECT_DEBUG
                string colorHex = ColorUtility.ToHtmlStringRGB(color ?? Color.yellow);
                Debug.LogWarning($"<color=#{colorHex}>{message}</color>", context);
#endif
        }

        public static void LogError(string message, Object context = null, Color? color = null)
        {
#if PROJECT_DEBUG
                string colorHex = ColorUtility.ToHtmlStringRGB(color ?? Color.red);
                Debug.LogError($"<color=#{colorHex}>{message}</color>", context);
#endif
        }
        
        public static void LogAssert(string message, Color? color = null)
        {
#if PROJECT_DEBUG
            Debug.LogAssertion($"<color=#{ColorUtility.ToHtmlStringRGB(color ?? Color.aquamarine)}>{message}</color>");
#endif
        }
    }
}