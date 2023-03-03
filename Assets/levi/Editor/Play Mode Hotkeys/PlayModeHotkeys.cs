#pragma warning disable IDE0051 // Remove unused private members. Unity calls them using MenuItem.
using UnityEditor;

namespace EditorPlus
{
    [InitializeOnLoad]
    public class PlayModeHotkeys : EditorWindow
    {
        private static bool startNextUpdate;
        private static bool startPaused;

        static PlayModeHotkeys()
        {
            EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
        }

        private static void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
        {
            if (startNextUpdate)
            {
                EditorApplication.isPaused = startPaused;
                EditorApplication.isPlaying = true;
            }
            startNextUpdate = false;
        }

        [MenuItem("Tools/Editor Pro/Play Mode Hotkeys/Toogle _F5")]
        static void Toogle()
        {
            EditorApplication.isPlaying = !EditorApplication.isPlaying;
        }

        [MenuItem("Tools/Editor Pro/Play Mode Hotkeys/Pause _F6")]
        static void Pause()
        {
            EditorApplication.isPaused = !EditorApplication.isPaused;
        }

        [MenuItem("Tools/Editor Pro/Play Mode Hotkeys/Next Frame _F7")]
        static void Next()
        {
            EditorApplication.Step();
        }

        [MenuItem("Tools/Editor Pro/Play Mode Hotkeys/Restart _F8")]
        static void Restart()
        {
            if (EditorApplication.isPlaying)
            {
                startPaused = EditorApplication.isPaused;
                EditorApplication.isPlaying = false;
                startNextUpdate = true;
            }
            else
            {
                EditorApplication.isPlaying = true;
            }
        }
    }
}
#pragma warning restore IDE0051 // Remove unused private members