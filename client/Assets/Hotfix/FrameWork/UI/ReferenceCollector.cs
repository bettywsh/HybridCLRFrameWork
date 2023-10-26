using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ReferenceData
{
    public Transform tranValue;
    public Image imgValue;
    public Text txtValue;
    public Button btnValue;
    public TMP_InputField tmpinputValue;
    public Toggle toggleValue;
    public Slider sliderValue;
    public TextMeshProUGUI tmptxtValue;
    public ListView listValue;
}

[Serializable]
public class ReferenceCollectorData
{
    public string name;

    public ReferenceData referenceData = new ReferenceData();
    //public Transform tranValue;
    //public Image imgValue;
    //public Text txtValue;
    //public Button btnValue;
    //public TMP_InputField tmpinputValue;
    //public Toggle toggleValue;
    //public Slider sliderValue;
    //public TextMeshProUGUI tmptxtValue;
    //public ListView listValue;
}


public class ReferenceCollector : MonoBehaviour
{
    [SerializeField]
    [HideInInspector]
    private int m_selectedIndex = -1;

    [SerializeField]
    [HideInInspector]
    public List<ReferenceCollectorData> data = new List<ReferenceCollectorData>();


    public void AutoBind()
    {
        data.Clear();
        var newData = new ReferenceCollectorData();
        newData.name = "transform";
        SetReferenceCollectorData(transform, newData);
        DeepSearch(transform);
    }

    private void DeepSearch(Transform tran)
    {
        if (tran.name[0] == '#' && tran.name[1] == '#')
            return;
        if (tran.name[0] == '#')
        {
            string objName = tran.name.Substring(1);
            string varType = objName.Split('_')[0];
            var newData = new ReferenceCollectorData();
            newData.name = objName;
            SetReferenceCollectorData(tran, newData);
            data.Add(newData);
        }
        for (int i = 0, count = tran.childCount; i < count; i++)
            DeepSearch(tran.GetChild(i));
    }

    void SetReferenceCollectorData(Transform tran, ReferenceCollectorData newData)
    {
        newData.referenceData.tranValue = tran;
        newData.referenceData.imgValue = tran.GetComponent<Image>();
        newData.referenceData.txtValue = tran.GetComponent<Text>();
        newData.referenceData.btnValue = tran.GetComponent<Button>();
        newData.referenceData.tmpinputValue = tran.GetComponent<TMP_InputField>();
        newData.referenceData.toggleValue = tran.GetComponent<Toggle>();
        newData.referenceData.sliderValue = tran.GetComponent<Slider>();
        newData.referenceData.tmptxtValue = tran.GetComponent<TextMeshProUGUI>();
        newData.referenceData.listValue = tran.GetComponent<ListView>();
    }

}
