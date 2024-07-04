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
    public Text txtValue;
    public Button btnValue;
    public TMP_InputField tmpinputValue;
    public Toggle toggleValue;
    public Slider sliderValue;
    public TextMeshProUGUI tmptxtValue;
    public CanvasGroup cngValue;
    public ListView listValue;
}


public class ReferenceCollector : SerializedMonoBehaviour
{
    //panel层代码路径
    private string PanelDir = "/Scripts/Hotfix/UI/Panel";
    //view层模版文件路径
    private string PanelTempletePath = "Assets/Editor/UI/TempPanel.bytes";


    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
    public Dictionary<string, ReferenceData> data = new Dictionary<string, ReferenceData>();

    [Button("自动绑定UI", buttonSize: ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
    public void AutoBind()
    {
        data.Clear();
        data.Add("transform", SetReferenceCollectorData(transform));
        DeepSearch(transform);
    }

    private void DeepSearch(Transform tran)
    {
        if (tran.name[0] == '#' && tran.name[1] == '#')
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
        newData.txtValue = tran.GetComponent<Text>();
        newData.btnValue = tran.GetComponent<Button>();
        newData.tmpinputValue = tran.GetComponent<TMP_InputField>();
        newData.toggleValue = tran.GetComponent<Toggle>();
        newData.sliderValue = tran.GetComponent<Slider>();
        newData.tmptxtValue = tran.GetComponent<TextMeshProUGUI>();
        newData.cngValue = tran.GetComponent<CanvasGroup>();
        newData.listValue = tran.GetComponent<ListView>();
        return newData;
    }

    public ReferenceData Get(string key)
    {
        ReferenceData referenceData;
        if (!data.TryGetValue(key, out referenceData))
        {
            return null;
        }
        return referenceData;
    }


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
}
