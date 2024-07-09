using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;

public class CellBase: IDisposable
{
    public Transform transform;
    public ReferenceCollector referenceCollector;
    public CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();


    public virtual void Init(Transform tf)
    {
        transform = tf;
        referenceCollector = transform.GetComponent<ReferenceCollector>();
    }

    public virtual void OnClose()
    {
        cancellationTokenSource.Cancel();
    }
    public virtual void Dispose() { 
    
    }
}
