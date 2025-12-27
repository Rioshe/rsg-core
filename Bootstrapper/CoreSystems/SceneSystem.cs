using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RSG
{
    public class SceneSystem : BootSystemBase
    {
        public override void Initialize()
        {
            SceneEvents.OnRequestSceneLoad += RequestSceneLoadEvent;
        }

        private void OnDestroy()
        {
            SceneEvents.OnRequestSceneLoad -= RequestSceneLoadEvent;
        }

        private void RequestSceneLoadEvent(string sceneName)
        {
            _ = LoadSceneAsyncInternal(sceneName);
        }

        public async Task LoadSceneAsync(string sceneName)
        {
            await LoadSceneAsyncInternal(sceneName);
        }

        private async Task LoadSceneAsyncInternal(string sceneName)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            if( op != null )
            {
                SceneEvents.RaiseSceneLoadStarted(sceneName);
                
                op.allowSceneActivation = false;

                while( op.progress < 0.9f )
                {
                    SceneEvents.RaiseSceneLoadProgress(sceneName, op.progress);
                    await Task.Yield();
                }

                op.allowSceneActivation = true;

                while( !op.isDone )
                {
                    SceneEvents.RaiseSceneLoadProgress(sceneName, op.progress);
                    await Task.Yield();
                }
                
                SceneEvents.RaiseSceneLoadComplete(sceneName);
            }
        }
    }
}