using RSG.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RSG.Debugger
{
    public partial class ProjectDebugger : MonoSingleton<ProjectDebugger>
    {
        public void Log(string message, Color color)
        {
#if PROJECT_DEBUG
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>");
#endif
        }
        
        public void LogError(string message)
        {
#if PROJECT_DEBUG
            Debug.LogError(message);
#endif
        }
        
        public void LogWarning(string message)
        {
#if PROJECT_DEBUG
            Debug.LogWarning(message);
#endif
        }
        
        public void LogAssert(string message, Color color)
        {
#if PROJECT_DEBUG
            Debug.LogAssertion($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}</color>");
#endif
        }
        
        public void MarkPosition(Vector3 position, int destroyAfterSeconds = 2, float scale = 0.1f)
        {
#if PROJECT_DEBUG
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = position;
            go.transform.localScale = Vector3.one * scale;
            go.transform.SetParent(transform);

            if (destroyAfterSeconds > 0)
            {
                Destroy(go, destroyAfterSeconds);
            }    
#endif
        }

        public void RestartScene()
        {
#if PROJECT_DEBUG
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
#endif
        }
    }
}