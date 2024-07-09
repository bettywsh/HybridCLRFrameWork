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

public class AotPanelBase: MonoBehaviour
{
    public object[] args;
    public CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    public virtual void OnOpen()
    { 
    
    }

    public virtual void Close()
    {
        AotUIManager.Instance.Close(this.GetType());
    }
}
