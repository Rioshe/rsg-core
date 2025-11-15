using UnityEditor;
using UnityEngine;

namespace RSG.Editor
{
    public static class PackageInstaller
    {
        [MenuItem("RSG/Install Essential Packages")]
        public static void Install()
        {
            Debug.Log("<color=cyan>[RSG]</color> Installing essential packages...");

            Install("https://github.com/Inspiaaa/UnityHFSM.git");
            Install("com.demigiant.dotween");
            Install("com.unity.in-game-debug-console"); // if in UPM registry
            Install("https://github.com/Tayx94/graphy.git");

            Debug.Log("<color=green>[RSG]</color> Essential packages installed!");
        }

        private static void Install(string package)
        {
            UnityEditor.PackageManager.Client.Add(package);
            Debug.Log($"<color=yellow>[RSG]</color> Installing: {package}");
        }
    }
}