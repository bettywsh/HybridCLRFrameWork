using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
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
}
