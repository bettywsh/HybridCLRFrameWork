using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class PlayModeState
{
    static PlayModeState()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            SingletonData.IsQuitting = false;
            SingletonData.PlaySession++;
            return;
        }

        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            SingletonData.IsQuitting = true;
            SingletonData.PlaySession++;
            return;
        }

        if (state != PlayModeStateChange.EnteredEditMode)
            return;

        var leftover = GameObject.Find("Singleton");
        if (leftover != null)
            Object.DestroyImmediate(leftover);
    }
}
