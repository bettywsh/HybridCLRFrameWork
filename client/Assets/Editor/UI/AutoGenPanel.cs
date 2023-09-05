using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using static Codice.CM.WorkspaceServer.WorkspaceTreeDataStore;
using TMPro;
using System;
using System.Reflection;
using System.ComponentModel;
using static UnityEditor.Progress;
using UnityEngine.UI;
using System.Threading.Tasks;

public class AutoGenPanel : Editor
{
    //panel层代码路径
    const string PanelDir = "Assets/Hotfix/UI/Panel";
    //view层模版文件路径
    const string PanelTempletePath = "Assets/Editor/UI/TempPanel.bytes";
    //view层代码路径
    const string ViewDir = "Assets/Hotfix/UI/View";
    //view层模版文件路径
    const string ViewTempletePath = "Assets/Editor/UI/TempPanelView.bytes";
    //字段模板
    const string FieldTemplete = "\tpublic {0} {1};";
    //{ get; set; }
    //字段模板
    const string PanelPrefab = "Assets/App/Prefab/UI/Panel/";

    static Dictionary<string, Transform> VarData = new Dictionary<string, Transform>();

    static Dictionary<string, VarType> VarTypes = new Dictionary<string, VarType>() {
        { "tsf", new VarType(){ name = "Transform", type = typeof(Transform) } },
        { "obj", new VarType(){ name = "GameObject", type = typeof(GameObject) } },
        { "txt", new VarType(){ name = "TextMeshProUGUI", type = typeof(TextMeshProUGUI) } },
        { "img", new VarType(){ name = "Image", type = typeof(Image) } },
        { "btn", new VarType(){ name = "Button", type = typeof(Button) } },
        { "tgl", new VarType(){ name = "Toggle", type = typeof(Toggle) } },
        { "sld", new VarType(){ name = "Slider", type = typeof(Slider) } },
        { "ipt", new VarType(){ name = "TMP_InputField", type = typeof(TMP_InputField) } },
        { "cng", new VarType(){ name = "CanvasGroup", type = typeof(CanvasGroup) } },
        { "lsv", new VarType(){ name = "ListView", type = typeof(ListView) } },
        //{ "tsf", new VarType(){ name = "Transform", type = typeof(Transform) } },
        //{ "tsf", new VarType(){ name = "Transform", type = typeof(Transform) } },
    };

    [MenuItem("AutoGenUI/CreatePanelAndView", false, 1)]
    public static void CreatePanelAndView()
    {
        VarData.Clear();
        GameObject go = Selection.activeGameObject;
        //判断是否是prefab
        if (PrefabUtility.GetPrefabAssetType(go) != PrefabAssetType.Regular)
        {
            Debug.LogWarning("选择的不是预制体，选择的对象：" + go.name);
            return;
        }
        if (!Directory.Exists(ViewDir))
        {
            Directory.CreateDirectory(ViewDir);
        }

        DeepSearch(go.transform);

        string className = go.name;
        StringBuilder fieldContent = new StringBuilder();

        foreach (KeyValuePair<string, Transform> item in VarData)
        {
            string tempFieldStr = FieldTemplete.Replace("{0}", GetNameToVarType(item.Key).name);
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
        Debug.Log("生成Panel完成");
    }

    [MenuItem("AutoGenUI/BindPanelView", false, 1)]
    static public void CreatePanelBind()
    {
        GameObject go = Selection.activeGameObject;
        VarData.Clear();
        DeepSearch(go.transform);

        Type type = Assembly.Load("Hotfix").GetType(go.name + "View");
        UnityEngine.Component com = go.GetComponent(type);
        if (com == null)
        {
            com = go.AddComponent(type);
        }
        FieldInfo[] allFieldInfo = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
        foreach (KeyValuePair<string, Transform> item in VarData)
        {
            Type type1 = GetNameToVarType(item.Key).type;
            UnityEngine.Component obj = item.Value.GetComponent(type1);
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
        Debug.Log("绑定Panel完成");

    }

    public static System.Type GetTypeByName(string name)
    {
        Debug.LogError(name);
        foreach (Assembly assembly in System.AppDomain.CurrentDomain.GetAssemblies())
        {
            //Debug.LogError(assembly.FullName);
            foreach (System.Type type in assembly.GetTypes())
            {
                if (type.Name == name)
                {
                    Debug.LogError(assembly.FullName);
                    return type;
                }
            }
        }

        return null;
    }

    private static void DeepSearch(Transform tran)
    {
        if (tran.name.Substring(0, 2) == "##")
            return;
        if (tran.name[0] == '#')
        {
            string objName = tran.name.Substring(1);
            string[] name = objName.Split("_");
            string[] typeName = name[0].Split(",");
            for (int i = 0; i < typeName.Length; i++)
            {
                string newname = typeName[i] + "_" + name[1];
                VarData.Add(newname, tran);
            }
      
        }
        for (int i = 0, count = tran.childCount; i < count; i++)
            DeepSearch(tran.GetChild(i));
    }


    public static VarType GetNameToVarType(string name)
    {
        name = name.Split("_")[0];
        VarType vt;
        VarTypes.TryGetValue(name, out vt);
        return vt;
    }
}

public class VarType
{
    public string name;
    public Type type;
}
