using RSG.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RSG.Debugger
{
    public partial class ProjectDebugger : MonoSingleton<ProjectDebugger>
    {
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