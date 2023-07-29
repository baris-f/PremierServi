using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;

namespace Modules.ScriptUtils.Editor
{
    [InitializeOnLoad]
    public class SceneSwitchLeftButton
    {
        static SceneSwitchLeftButton() => ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);

        private static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(new GUIContent("Start Scene", "Start Scene with build index 0")))
                StartScene();
        }

        private static void StartScene()
        {
            if (EditorApplication.isPlaying)
                EditorApplication.isPlaying = false;
            EditorApplication.update += OnUpdate;
        }

        private static void OnUpdate()
        {
            if (EditorApplication.isPlaying || EditorApplication.isPaused ||
                EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
                return;
            EditorApplication.update -= OnUpdate;
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;
            var scenePath = EditorBuildSettings.scenes[0].path;
            EditorSceneManager.OpenScene(scenePath);
            EditorApplication.isPlaying = true;
        }
    }
}