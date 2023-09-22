using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AutoGenSubPanel : Editor
{
    //view层代码路径
    const string ViewDir = "Assets/Hotfix/UI/SubView";

    [MenuItem("AutoGenUI/CreateSubPanelAndView", false, 3)]
    public static void CreateSubPanelAndView()
    {
        AutoGenPanel.VarData.Clear();
        GameObject go = Selection.activeGameObject;
        //判断是否是prefab
        if (go == null)
        {
            Debug.LogWarning("选择的不是GameObject");
            return;
        }
        if (!Directory.Exists(ViewDir))
        {
            Directory.CreateDirectory(ViewDir);
        }

        DeepSearch(go.transform);
    }
}
