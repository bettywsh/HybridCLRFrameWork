using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;

public class ReplaceFont : EditorWindow
{
    public static string ReplacePrefabPath = "Assets/App/Prefab";
    public static string FootPath = "Assets/App/Font/SourceHanSerifCN-BoldSDF.asset";

    [MenuItem("Tools/ReplaceFont")]
    public static void ReplaceFontPrefab()
    {
        string[] files = AssetDatabase.FindAssets("t:Prefab", new[] { ReplacePrefabPath });
        foreach (string file in files)
        {
            string tmpPath = AssetDatabase.GUIDToAssetPath(file);
            GameObject tmpObj = AssetDatabase.LoadAssetAtPath(tmpPath, typeof(GameObject)) as GameObject;
            TextMeshProUGUI[] tmpText = tmpObj.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in tmpText)
            {
                text.font = AssetDatabase.LoadAssetAtPath(FootPath, typeof(TMP_FontAsset)) as TMP_FontAsset;
            }
        }
    }
}
