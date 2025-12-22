using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RSG
{
    public class SceneSystem : BootSystemBase
    {
        public override void Initialize() { }

        public async Task LoadSceneAsync(string sceneName)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
            if( op != null )
            {
                op.allowSceneActivation = false;

                while( op.progress < 0.9f )
                    await Task.Yield();

                op.allowSceneActivation = true;

                while( !op.isDone )
                    await Task.Yield();
            }
        }
    }
}