using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AutoGenSubPanel : Editor
{
    //view�����·��
    const string ViewDir = "Assets/Hotfix/UI/SubView";

    [MenuItem("AutoGenUI/CreateSubPanelAndView", false, 3)]
    public static void CreateSubPanelAndView()
    {
        AutoGenPanel.VarData.Clear();
        GameObject go = Selection.activeGameObject;
        //�ж��Ƿ���prefab
        if (go == null)
        {
            Debug.LogWarning("ѡ��Ĳ���GameObject");
            return;
        }
        if (!Directory.Exists(ViewDir))
        {
            Directory.CreateDirectory(ViewDir);
        }

        DeepSearch(go.transform);
    }
}
