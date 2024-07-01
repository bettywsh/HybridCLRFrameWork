using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetLoadingPanel : BasePanel
{
    public override void OnBindEvent()
    {
        base.OnBindEvent();
    }
    public override async UniTask OnOpen()
    { 
        await base.OnOpen();
    }

    public override void OnUnBindEvent()
    { 
        base.OnUnBindEvent();
    }

    public override void OnClose()
    {
        cancellationTokenSource.Cancel();
    }
}
