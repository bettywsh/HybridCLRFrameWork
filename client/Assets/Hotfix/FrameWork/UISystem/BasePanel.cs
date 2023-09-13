using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using com.bochsler.protocol;


public class BasePanel
{
    public Transform transform;
    public object[] args;
    
    public virtual void OnBindEvent()
    {
        EventHelper.RegisterEvent(this);
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
