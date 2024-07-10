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

public class SubPanelBase : IDisposable
{
    public ReferenceCollector referenceCollector;
    public Transform transform;
    public object[] args;
    public CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    public virtual void OnBindEvent()
    {
        referenceCollector = transform.GetComponent<ReferenceCollector>();

        EventHelper.RegisterAllEvent(this, referenceCollector);
    }

    public virtual async UniTask OnOpen()
    {
        await UniTask.CompletedTask;
    }

    public virtual void Close()
    {
        OnUnBindEvent();
        OnClose();
        Dispose();
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

    }
}
