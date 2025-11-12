using UnityEngine;

namespace RSG
{
    public static class TransformExtensions
    {
        private static Camera s_camera;

        public static bool IsOnScreen(this Transform transform)
        {
            if(!s_camera)
            {
                s_camera = Camera.main;
                if (!s_camera)
                {
                    Debug.LogError("Main camera not found. Please ensure a camera is tagged as 'MainCamera'.");
                    return false;
                }
            }
            
            Vector3 viewport = s_camera.WorldToViewportPoint(transform.position);
            return viewport.x is >= -0.1f and <= 1.1f &&
                   viewport.y is >= -0.1f and <= 1.1f;
        }
    }
    
}