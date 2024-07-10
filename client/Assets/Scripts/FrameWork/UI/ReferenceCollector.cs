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
    public LoadSubPanel loadSubPanelValue;
}


public class ReferenceCollector : SerializedMonoBehaviour
{
    //Panel�����·��
    private string PanelDir = "/Scripts/Hotfix/UI/Panel";
    //view��ģ���ļ�·��
    private string PanelTempletePath = "Assets/Editor/UI/TempPanel.bytes";

    //SubPanel�����·��
    private string SubPanelDir = "/Scripts/Hotfix/UI/SubPanel";
    //view��ģ���ļ�·��
    private string SubPanelTempletePath = "Assets/Editor/UI/TempSubPanel.bytes";

    //SubPanel�����·��
    private string CellDir = "/Scripts/Hotfix/UI/Cell";
    //view��ģ���ļ�·��
    private string CellTempletePath = "Assets/Editor/UI/TempCell.bytes";


    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
    public Dictionary<string, ReferenceData> data = new Dictionary<string, ReferenceData>();

    [Button("�Զ���UI", buttonSize: ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
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
        newData.txtValue = tran.GetComponent<Text>();
        newData.btnValue = tran.GetComponent<Button>();
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
            return null;
        }
        return referenceData;
    }


    [Button("����Panel.cs", buttonSize: ButtonSizes.Large), GUIColor(0, 1, 0)]
    public void CreatePanel()
    {
        var fullFilePath = EditorUtility.SaveFilePanel($"Please select a folder to create", Application.dataPath + PanelDir, transform.name, "cs");

        if (File.Exists(fullFilePath))
        {
            Debug.LogError("�ļ��Ѵ���");
            return;
        }
 
        string tempcs = AssetDatabase.LoadAssetAtPath<TextAsset>(PanelTempletePath).text;
        tempcs = tempcs.Replace("#CLASSNAME#", transform.name);
        byte[] buffer1 = Encoding.Default.GetBytes(tempcs.ToString());
        byte[] buffer2 = Encoding.Convert(Encoding.UTF8, Encoding.Default, buffer1, 0, buffer1.Length);
        File.WriteAllBytes(fullFilePath, buffer2);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("�ɹ�", "����Panel�ɹ�!!!", "֪����");
    }

    [Button("����SubPanel.cs", buttonSize: ButtonSizes.Large), GUIColor(0, 1, 0)]
    public void CreateSubPanel()
    {
        var fullFilePath = EditorUtility.SaveFilePanel($"Please select a folder to create", Application.dataPath + SubPanelDir, transform.name, "cs");

        if (File.Exists(fullFilePath))
        {
            Debug.LogError("�ļ��Ѵ���");
            return;
        }

        string tempcs = AssetDatabase.LoadAssetAtPath<TextAsset>(SubPanelTempletePath).text;
        tempcs = tempcs.Replace("#CLASSNAME#", transform.name);
        byte[] buffer1 = Encoding.Default.GetBytes(tempcs.ToString());
        byte[] buffer2 = Encoding.Convert(Encoding.UTF8, Encoding.Default, buffer1, 0, buffer1.Length);
        File.WriteAllBytes(fullFilePath, buffer2);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("�ɹ�", "����SubPanel�ɹ�!!!", "֪����");
    }

    [Button("����Cell.cs", buttonSize: ButtonSizes.Large), GUIColor(0, 1, 0)]
    public void CreateCell()
    {
        var fullFilePath = EditorUtility.SaveFilePanel($"Please select a folder to create", Application.dataPath + CellDir, transform.name, "cs");

        if (File.Exists(fullFilePath))
        {
            Debug.LogError("�ļ��Ѵ���");
            return;
        }

        string tempcs = AssetDatabase.LoadAssetAtPath<TextAsset>(CellTempletePath).text;
        tempcs = tempcs.Replace("#CLASSNAME#", transform.name);
        byte[] buffer1 = Encoding.Default.GetBytes(tempcs.ToString());
        byte[] buffer2 = Encoding.Convert(Encoding.UTF8, Encoding.Default, buffer1, 0, buffer1.Length);
        File.WriteAllBytes(fullFilePath, buffer2);
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("�ɹ�", "����Cell�ɹ�!!!", "֪����");
    }
}
