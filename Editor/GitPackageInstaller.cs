using UnityEditor;
using UnityEngine;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace RSG.Editor
{
    public static class GitPackageInstaller
    {
        // --- List of packages to install from Git ---
        private static readonly string[] packages = new[]
        {
            "https://github.com/Tayx94/graphy.git",
            "https://github.com/yasirkula/UnityIngameDebugConsole.git"
            // Add any other Git URLs here
        };

        // --- Installation Logic ---

        private static int index = 0;
        private static AddRequest currentRequest;

        [MenuItem("RSG/Install Git Packages")]
        public static void Install()
        {
            Debug.Log("<color=cyan>[RSG]</color> Installing essential packages...");
            index = 0;
            InstallNext();
        }

        private static void InstallNext()
        {
            if (index >= packages.Length)
            {
                Debug.Log("<color=green>[RSG]</color> All essential packages installed!");
                return;
            }

            string package = packages[index];
            Debug.Log($"<color=yellow>[RSG]</color> Installing: {package}");

            currentRequest = Client.Add(package);
            EditorApplication.update += Progress;
        }

        private static void Progress()
        {
            if (currentRequest.IsCompleted)
            {
                EditorApplication.update -= Progress;

                if (currentRequest.Status == StatusCode.Success)
                {
                    Debug.Log($"<color=lime>[RSG]</color> Installed: {currentRequest.Result.packageId}");
                }
                else
                {
                    Debug.LogError($"<color=red>[RSG]</color> Failed to install '{packages[index]}': {currentRequest.Error.message}");
                }

                index++;
                InstallNext();
            }
        }
    }
}