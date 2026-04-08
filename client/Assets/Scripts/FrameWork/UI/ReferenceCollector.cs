using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


[InlineProperty(LabelWidth = 90)]
public class ReferenceData
{
    public Transform tranValue;
    public GameObject goValue;
    public Image imgValue;
    public UButton btnValue;
    public TMP_InputField tmpinputValue;
    public Toggle toggleValue;
    public Slider sliderValue;
    public TextMeshProUGUI tmptxtValue;
    public CanvasGroup cngValue;
    public ListView listValue;
    public LoadSubPanel loadSubPanelValue;
}


public class ReferenceCollector : SerializedMonoBehaviour
{
    //Panel代码路径
    private string PanelDir = "/Scripts/Hotfix/UI/Panel";
    //Panel模版文件路径
    private string PanelTempletePath = "Assets/Scripts/Editor/UI/TempPanel.bytes";

    //Data代码路径
    private string DataDir = "/Scripts/Hotfix/Data";
    //Data模版文件路径
    private string DataTempletePath = "Assets/Scripts/Editor/UI/TempData.bytes";

    //SubPanel层代码路径
    private string SubPanelDir = "/Scripts/Hotfix/UI/SubPanel";
    //SubPanel模版文件路径
    private string SubPanelTempletePath = "Assets/Scripts/Editor/UI/TempSubPanel.bytes";

    //Cell层代码路径
    private string CellDir = "/Scripts/Hotfix/UI/Cell";
    //Cell模版文件路径
    private string CellTempletePath = "Assets/Scripts/Editor/UI/TempCell.bytes";

    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
    public Dictionary<string, ReferenceData> data = new Dictionary<string, ReferenceData>();

    [Button("自动绑定UI", buttonSize: ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
    public void AutoBind()
    {
        data.Clear();
        data.Add("transform", SetReferenceCollectorData(transform));
        //DeepSearch(transform);
        for (int i = 0, count = transform.childCount; i < count; i++)
            DeepSearch(transform.GetChild(i));
    }

    private void DeepSearch(Transform tran)
    {
        if (tran.GetComponent<ReferenceCollector>() != null)
            return;
        if (tran.name[0] == '#')
        {
            string objName = tran.name.Substring(1);
            data.Add(objName, SetReferenceCollectorData(tran));
        }
        for (int i = 0, count = tran.childCount; i < count; i++)
            DeepSearch(tran.GetChild(i));
    }

    ReferenceData SetReferenceCollectorData(Transform tran)
    {
        ReferenceData newData = new ReferenceData();
        newData.tranValue = tran;
        newData.goValue = tran.gameObject;
        newData.imgValue = tran.GetComponent<Image>();
        newData.btnValue = tran.GetComponent<UButton>();
        newData.tmpinputValue = tran.GetComponent<TMP_InputField>();
        newData.toggleValue = tran.GetComponent<Toggle>();
        newData.sliderValue = tran.GetComponent<Slider>();
        newData.tmptxtValue = tran.GetComponent<TextMeshProUGUI>();
        newData.cngValue = tran.GetComponent<CanvasGroup>();
        newData.listValue = tran.GetComponent<ListView>();
        newData.loadSubPanelValue = tran.GetComponent<LoadSubPanel>();
        return newData;
    }

    public ReferenceData Get(string key)
    {
        ReferenceData referenceData;
        if (!data.TryGetValue(key, out referenceData))
        {
            Debug.LogError($"UI找不到key{key}");
            return null;
        }
        return referenceData;
    }

#if UNITY_EDITOR
    [ShowIf("@transform.name.Contains(\"Panel\") && transform.name.Contains(\"SubPanel\") == false")]
    [Button("创建Panel.cs", buttonSize: ButtonSizes.Large), GUIColor(0, 1, 0)]
    public void CreatePanel()
    {
        var fullFilePath = EditorUtility.SaveFilePanel($"Please select a folder to create", Application.dataPath + PanelDir, transform.name, "cs");

        if (File.Exists(fullFilePath))
        {
            Debug.LogError("文件已存在");
            return;
        }
 
        string tempcs = AssetDatabase.LoadAssetAtPath<TextAsset>(PanelTempletePath).text;
        tempcs = tempcs.Replace("#CLASSNAME#", transform.name);
        byte[] buffer1 = Encoding.Default.GetBytes(tempcs.ToString());
        byte[] buffer2 = Encoding.Convert(Encoding.UTF8, Encoding.Default, buffer1, 0, buffer1.Length);
        File.WriteAllBytes(fullFilePath, buffer2);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("成功", "创建Panel成功!!!", "知道了");
    }

    [ShowIf("@transform.name.Contains(\"Panel\") && transform.name.Contains(\"SubPanel\") == false")]
    [Button("创建Data.cs", buttonSize: ButtonSizes.Large), GUIColor(0, 1, 0)]
    public void CreateData()
    {
        var fullFilePath = EditorUtility.SaveFilePanel($"Please select a folder to create", Application.dataPath + DataDir, transform.name.Replace("Panel","Data"), "cs");

        if (File.Exists(fullFilePath))
        {
            Debug.LogError("文件已存在");
            return;
        }

        string tempcs = AssetDatabase.LoadAssetAtPath<TextAsset>(DataTempletePath).text;
        tempcs = tempcs.Replace("#CLASSNAME#", transform.name);
        byte[] buffer1 = Encoding.Default.GetBytes(tempcs.ToString());
        byte[] buffer2 = Encoding.Convert(Encoding.UTF8, Encoding.Default, buffer1, 0, buffer1.Length);
        File.WriteAllBytes(fullFilePath, buffer2);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("成功", "创建Data成功!!!", "知道了");
    }

    [ShowIf("@transform.name.Contains(\"SubPanel\")")]
    [Button("创建SubPanel.cs", buttonSize: ButtonSizes.Large), GUIColor(0, 1, 0)]
    public void CreateSubPanel()
    {
        var fullFilePath = EditorUtility.SaveFilePanel($"Please select a folder to create", Application.dataPath + SubPanelDir, transform.name, "cs");

        if (File.Exists(fullFilePath))
        {
            Debug.LogError("文件已存在");
            return;
        }

        string tempcs = AssetDatabase.LoadAssetAtPath<TextAsset>(SubPanelTempletePath).text;
        tempcs = tempcs.Replace("#CLASSNAME#", transform.name);
        byte[] buffer1 = Encoding.Default.GetBytes(tempcs.ToString());
        byte[] buffer2 = Encoding.Convert(Encoding.UTF8, Encoding.Default, buffer1, 0, buffer1.Length);
        File.WriteAllBytes(fullFilePath, buffer2);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("成功", "创建SubPanel成功!!!", "知道了");
    }

    [ShowIf("@transform.name.Contains(\"Cell\")")]
    [Button("创建Cell.cs", buttonSize: ButtonSizes.Large), GUIColor(0, 1, 0)]
    public void CreateCell()
    {
        var fullFilePath = EditorUtility.SaveFilePanel($"Please select a folder to create", Application.dataPath + CellDir, transform.name, "cs");

        if (File.Exists(fullFilePath))
        {
            Debug.LogError("文件已存在");
            return;
        }

        string tempcs = AssetDatabase.LoadAssetAtPath<TextAsset>(CellTempletePath).text;
        tempcs = tempcs.Replace("#CLASSNAME#", transform.name);
        byte[] buffer1 = Encoding.Default.GetBytes(tempcs.ToString());
        byte[] buffer2 = Encoding.Convert(Encoding.UTF8, Encoding.Default, buffer1, 0, buffer1.Length);
        File.WriteAllBytes(fullFilePath, buffer2);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("成功", "创建Cell成功!!!", "知道了");
    }
#endif
 
}
