using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

public class AutoGenSubPanel : Editor
{
    //panel层代码路径
    const string PanelDir = "Assets/Hotfix/UI/SubPanel";
    //view层模版文件路径
    const string PanelTempletePath = "Assets/Editor/UI/TempSubPanel.bytes";
    //view层代码路径
    const string ViewDir = "Assets/Hotfix/UI/SubView";
    //view层模版文件路径
    const string ViewTempletePath = "Assets/Editor/UI/TempSubPanelView.bytes";
    //字段模板
    const string FieldTemplete = "\tpublic {0} {1};";
    //{ get; set; }
    //字段模板
    const string PanelPrefab = "Assets/App/Prefab/UI/SubPanel/";


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

        AutoGenPanel.DeepSearch(go.transform);

        string className = go.name;
        StringBuilder fieldContent = new StringBuilder();

        foreach (KeyValuePair<string, Transform> item in AutoGenPanel.VarData)
        {
            string tempFieldStr = FieldTemplete.Replace("{0}", AutoGenPanel.GetNameToVarType(item.Key).name);
            tempFieldStr = tempFieldStr.Replace("{1}", item.Key);
            fieldContent.AppendLine(tempFieldStr);
        }
        string viewTempleteContent = File.ReadAllText(ViewTempletePath, Encoding.UTF8);
        string viewPath = ViewDir + "/" + go.name + "View.cs";
        if (File.Exists(viewPath))
        {
            File.Delete(viewPath);
        }
        AssetDatabase.Refresh();
        using (StreamWriter sw = new StreamWriter(viewPath))
        {
            viewTempleteContent = viewTempleteContent.Replace("#CLASSNAME#", className + "View");
            viewTempleteContent = viewTempleteContent.Replace("#FIELD_BIND#", fieldContent.ToString());
            sw.Write(viewTempleteContent);
            sw.Close();
        }

        string panelTempleteContent = File.ReadAllText(PanelTempletePath, Encoding.UTF8);
        string panelPath = PanelDir + "/" + go.name + ".cs";
        if (!File.Exists(panelPath))
        {
            using (StreamWriter sw = new StreamWriter(panelPath))
            {
                panelTempleteContent = panelTempleteContent.Replace("#CLASSNAME#", className);
                sw.Write(panelTempleteContent);
                sw.Close();
            }
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        Debug.Log("生成SubPanel完成");
    }

    [MenuItem("AutoGenUI/BindSubPanelView", false, 1)]
    static public void CreatePanelBind()
    {
        GameObject go = Selection.activeGameObject;
        AutoGenPanel.VarData.Clear();
        AutoGenPanel.DeepSearch(go.transform);

        Type type = Assembly.Load("Hotfix").GetType(go.name + "View");
        UnityEngine.Component com = go.GetComponent(type);
        if (com == null)
        {
            com = go.AddComponent(type);
        }
        FieldInfo[] allFieldInfo = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
        foreach (KeyValuePair<string, Transform> item in AutoGenPanel.VarData)
        {
            Type type1 = AutoGenPanel.GetNameToVarType(item.Key).type;
            object obj = item.Value.gameObject;
            if (type1.Name != "GameObject")
            {
                obj = item.Value.GetComponent(type1) as object;
            }
            for (int i = 0; i < allFieldInfo.Length; i++)
            {
                if (allFieldInfo[i].Name == item.Key)
                {
                    allFieldInfo[i].SetValue(com, obj);
                }
            }
        }
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        GameObject newgo = GameObject.Instantiate(go);
        UnityEditor.PrefabUtility.SaveAsPrefabAssetAndConnect(newgo, PanelPrefab + go.name + ".prefab", UnityEditor.InteractionMode.UserAction);
        GameObject.DestroyImmediate(newgo);
        AssetDatabase.SaveAssets();
        Debug.Log("绑定SubPanel完成");

    }
}
