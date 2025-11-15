using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace RSG.Editor
{
    public static class UpdateGitignore
    {
        private const string GitignoreFileName = ".gitignore";
        private const string GitignoreContentName = "GitIgnoreContent"; 

        [MenuItem("RSG/Update Gitignore")]
        public static void UpdateOrCreateGitignore()
        {
            string projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            string gitignorePath = Path.Combine(projectRoot, GitignoreFileName);

            Debug.Log("<color=cyan>[RSG]</color> Updating .gitignore...");

            // Locate template
            string templatePath = FindTemplatePath();
            if (templatePath == null)
            {
                Debug.LogError("[RSG] Could not find GitIgnoreContent file!");
                return;
            }

            string[] templateLines = File.ReadAllLines(templatePath)
                .Select(l => l.TrimEnd())   // remove trailing spaces
                .ToArray();

            // Ensure .gitignore exists
            if (!File.Exists(gitignorePath))
            {
                Debug.LogWarning("[RSG] .gitignore not found. Creating a new one...");
                File.WriteAllLines(gitignorePath, templateLines);
                Debug.Log("<color=green>[RSG]</color> Created new .gitignore");
                return;
            }

            // Merge into existing file
            List<string> existing = File.ReadAllLines(gitignorePath)
                .Select(l => l.TrimEnd())
                .ToList();

            int addedCount = 0;

            foreach (string line in templateLines)
            {
                // skip empties at top to avoid spam
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (existing.Count == 0 || existing.Last() != "")
                        existing.Add("");
                    continue;
                }

                if (!existing.Contains(line))
                {
                    existing.Add(line);
                    addedCount++;
                    Debug.Log($"<color=yellow>[RSG]</color> Added: {line}");
                }
            }

            File.WriteAllLines(gitignorePath, existing);

            Debug.Log($"<color=green>[RSG]</color> Gitignore updated. Added {addedCount} new entries.");
        }



        private static string FindTemplatePath()
        {
            string[] guids = AssetDatabase.FindAssets(GitignoreContentName);

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (Path.GetFileNameWithoutExtension(path) == GitignoreContentName)
                    return Path.GetFullPath(path);
            }

            return null;
        }
    }
}
