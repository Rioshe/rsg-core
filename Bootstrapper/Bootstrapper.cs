using System;
using UnityEngine;

namespace RSG
{
    [DefaultExecutionOrder(-100)]
    public class Bootstrapper : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad( gameObject );
        }

        private async void Start()
        {
            try
            {
                IService[] services = GetComponentsInChildren<IService>();
                foreach( IService service in services )
                {
                    await service.InitializeAsync();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        private async void OnDestroy()
        {
            try
            {
                IService[] services = GetComponentsInChildren<IService>();
                foreach( IService service in services )
                {
                    await service.ShutdownAsync();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}