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
    
    public virtual async UniTask OnBindEvent()
    {
        referenceCollector = transform.GetComponent<ReferenceCollector>();
        referenceData = referenceCollector.data.ToDictionary(x => x.name, x => x.referenceData);
        EventHelper.RegisterEvent(this, referenceData);
    }

    public virtual async UniTask OnOpen()
    {

    }

    public virtual void OnUpdate()
    { 
    
    }

    public virtual void Close()
    {
        //UIManager.Instance.Close<this>();
    }

    public virtual void OnClose()
    {
        EventHelper.UnRegisterEvent(this);
    }
}
