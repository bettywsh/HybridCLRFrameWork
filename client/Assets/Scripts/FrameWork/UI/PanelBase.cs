using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;
using Cysharp.Threading.Tasks;
using System.Threading;

public class PanelBase : IDisposable
{
    public ReferenceCollector referenceCollector;
    public Dictionary<string, ReferenceData> referenceData;
    public Transform transform;
    public object[] args;
    public CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    public virtual void OnBindEvent()
    {
        referenceCollector = transform.GetComponent<ReferenceCollector>();
        referenceData = referenceCollector.data;

        EventHelper.RegisterAllEvent(this, referenceData);
    }

    public virtual async UniTask OnOpen()
    {
        await UniTask.CompletedTask;
    }

    public virtual void OnUpdate()
    {

    }

    public virtual void Close()
    {
        UIManager.Instance.Close(this.GetType());
    }

    public virtual void OnUnBindEvent()
    {
        EventHelper.UnRegisterAllEvent(this);
    }

    public virtual void OnClose()
    {
        cancellationTokenSource.Cancel();        
    }

    public virtual void Dispose()
    {
        OnClose();
    }
}
