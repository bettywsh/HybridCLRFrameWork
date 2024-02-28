using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using Cysharp.Threading.Tasks;

public class BasePanel
{
    public ReferenceCollector referenceCollector;
    public Dictionary<string, ReferenceData> referenceData;
    public Transform transform;
    public object[] args;


    public List<string> messagesList = new List<string>();
    public List<int> netsList = new List<int>();
    public List<Button> clickList = new List<Button>();

    public virtual async UniTask OnBindEvent()
    {
        referenceCollector = transform.GetComponent<ReferenceCollector>();
        referenceData = referenceCollector.data.ToDictionary(x => x.name, x => x.referenceData);

        //object[] atts = this.GetType().GetCustomAttributes(false);
        foreach (MethodInfo method in this.GetType().GetMethods())
        {
            Debug.Log(method.Name);
            foreach (var att in method.GetCustomAttributes(true))
            {
                if (att is OnClickAttribute) {
                    ReferenceData btn;
                    referenceData.TryGetValue((att as OnClickAttribute).Name, out btn);
                    btn.btnValue.onClick.AddListener(() => { method.Invoke(this, null); });
                    clickList.Add(btn.btnValue);
                }
                else if (att is OnNetAttribute) {
                    int id = (att as OnNetAttribute).Id;
                    MessageManager.Instance.RegisterNetMessageHandler(id, (msgDatas) => { method.Invoke(this, null); });
                    netsList.Add(id);
                }
                else if (att is OnMessageAttribute) {
                    MessageManager.Instance.RegisterMessageHandler((att as OnMessageAttribute).Name, (msgDatas) => { method.Invoke(this, null); });
                    messagesList.Add((att as OnMessageAttribute).Name);
                }
            }
        }
    }

    public virtual async UniTask OnOpen()
    {

    }

    public virtual void OnUpdate()
    { 
    
    }

    public virtual void Close()
    {
        UIManager.Instance.Close(this.GetType());
    }

    public virtual void OnClose()
    {
        foreach (var item in messagesList){
            MessageManager.Instance.RemoveMessage(item);
        }
        foreach (var item in netsList)
        {
            MessageManager.Instance.RemoveNetMessage(item);
        }
        foreach (var item in clickList) { 
            item.onClick.RemoveAllListeners();
        }
    }
}
