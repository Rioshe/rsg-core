using UnityEditor;
using System.IO;

namespace RSG.Core.Editor
{
    public class ProjectFoldersCreator
    {
        [MenuItem("RSG/Create Standard Folders")]
        public static void CreateFolders()
        {
            string[] folders = {
                "Assets/_Project/Art/Animations",
                "Assets/_Project/Art/Materials",
                "Assets/_Project/Art/Models",
                "Assets/_Project/Art/Sprites",
                "Assets/_Project/Audio/Music",
                "Assets/_Project/Audio/SFX",
                "Assets/_Project/Prefabs/UI",
                "Assets/_Project/Prefabs/Gameplay",
                "Assets/_Project/Scenes",
                "Assets/_Project/Scripts/Core",
                "Assets/_Project/Scripts/Gameplay",
                "Assets/_Project/Scripts/UI",
                "Assets/_Project/Shaders",
                "Assets/_Project/UI/Elements",
                "Assets/_Project/UI/Layouts",
                "Assets/_Project/UI/Fonts",
                "Assets/_Project/UI/Sprites",
                "Assets/_Sandbox",
                "Assets/_ThirdParty"
            };
        
            foreach (string folder in folders)
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);
        
            AssetDatabase.Refresh();
        }
    }

}
