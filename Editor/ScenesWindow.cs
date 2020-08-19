using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Playrika.GameFoundation.Editor
{
    public class ScenesWindow : EditorWindow
    {
        private Vector2 _scrollPosition;

        [MenuItem("GameFoundation/Windows/Scenes")]
        public static void ShowWindow()
        {
            var window = GetWindow<ScenesWindow>("Scenes");
            window.minSize = new Vector2(0f, 0f);
        }

        private void OnGUI()
        {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandWidth(true));

            foreach (var editorBuildSettingsScene in EditorBuildSettings.scenes)
            {
                var separatedPath = editorBuildSettingsScene.path.Split('/');
                var sceneName = separatedPath[separatedPath.Length - 1];
                sceneName = sceneName.Remove(sceneName.Length - 6);

                if (GUILayout.Button(sceneName))
                {
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    EditorSceneManager.OpenScene(editorBuildSettingsScene.path);
                }
            }

            GUILayout.EndScrollView();
        }
    }
}