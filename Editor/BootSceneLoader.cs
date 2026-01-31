using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RSG.Editor
{
    [InitializeOnLoad]
    public static class BootSceneLoader
    {
        private const string MenuPath = "RSG/Force Boot Scene on Play";
        private const string PrefsKey = "BootSceneLoader.Enabled";

        private static bool IsEnabled
        {
            get => EditorPrefs.GetBool( PrefsKey, true );
            set => EditorPrefs.SetBool( PrefsKey, value );
        }

        static BootSceneLoader()
        {
            EditorApplication.playModeStateChanged += LoadDefaultScene;
        }

        private static void LoadDefaultScene( PlayModeStateChange state )
        {
            if( !IsEnabled )
            {
                return;
            }

            if( state == PlayModeStateChange.ExitingEditMode )
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            }

            if( state == PlayModeStateChange.EnteredPlayMode )
            {
                if( UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0 )
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene( 0 );
                }
            }
        }

        [MenuItem( MenuPath )]
        private static void ToggleBootSceneLoading()
        {
            IsEnabled = !IsEnabled;
            Debug.Log( $"'{MenuPath}' is now {(IsEnabled ? "ON" : "OFF")}" );
        }

        [MenuItem( MenuPath, true )]
        private static bool ToggleBootSceneLoading_Validate()
        {
            Menu.SetChecked( MenuPath, IsEnabled );
            return true;
        }
    }
}