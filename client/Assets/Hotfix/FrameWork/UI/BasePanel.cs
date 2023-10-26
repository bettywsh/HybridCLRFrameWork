using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using com.bochsler.protocol;
using System.Linq;

public class BasePanel
{
    public ReferenceCollector referenceCollector;
    public Dictionary<string, ReferenceData> referenceData;
    public Transform transform;
    public object[] args;
    
    public virtual void OnBindEvent()
    {
        referenceCollector = transform.GetComponent<ReferenceCollector>();
        referenceData = referenceCollector.data.ToDictionary(x => x.name, x => x.referenceData);
        EventHelper.RegisterEvent(this, referenceData);
    }

    public virtual void OnOpen()
    {

    }

    public virtual void OnUpdate()
    { 
    
    }

    public virtual void OnClose()
    {
        EventHelper.UnRegisterEvent(this);
    }
}
