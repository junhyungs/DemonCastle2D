using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace DemonCastle.EditorTools
{
    [InitializeOnLoad]
    public static class BootstrapPlayModeStarter
    {
        private const string BootstrapScenePath = "Assets/Scenes/Bootstrap.unity";
        
        static BootstrapPlayModeStarter()
        {
            SceneAsset bootstrapScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(BootstrapScenePath);
            if(bootstrapScene == null)
            {
                Debug.LogWarning($"[BootstrapPlayModeStarter]'{BootstrapScenePath}'에서 Bootstrap 씬을 찾지 못했습니다. 열려 있는 씬에서 바로 시작합니다.");
                EditorSceneManager.playModeStartScene = null;
                return;
            }

            EditorSceneManager.playModeStartScene = bootstrapScene;
        }
    }
}


