using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RSG
{
    public class SceneService : MonoBehaviour, ISceneService
    {
        public Task InitializeAsync()
        {
            ServiceLocator.Register<ISceneService>(this);
            return Task.CompletedTask;
        }
        public Task ShutdownAsync()
        {
            ServiceLocator.Unregister<ISceneService>(this);
            return Task.CompletedTask;
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
                
                op.allowSceneActivation = false;

                while( op.progress < 0.9f )
                {
                    await Task.Yield();
                }

                op.allowSceneActivation = true;

                while( !op.isDone )
                {
                    await Task.Yield();
                }
            }
        }
    }
}