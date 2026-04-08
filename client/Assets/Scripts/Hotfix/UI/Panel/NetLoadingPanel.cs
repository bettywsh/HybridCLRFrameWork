using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class NetLoadingPanel : PanelBase
{
    public override void OnBindEvent()
    {
        base.OnBindEvent();
    }
    public override async UniTask OnOpen()
    { 
        await base.OnOpen();
        transform.GetComponent<Canvas>().sortingOrder = (int) EUILayer.NetLoding;


    }

    public override void OnUnBindEvent()
    { 
        base.OnUnBindEvent();
    }
    public async void Show()
    {
        cancellationTokenSource = new CancellationTokenSource();
        transform.gameObject.SetActive(true);
        GetUI("MainPanel").goValue.SetActive(false);
        bool cancel = await UniTask.Delay(3000, cancellationToken: cancellationTokenSource.Token).SuppressCancellationThrow();
        if (cancel)
            return;
        GetUI("MainPanel").goValue.SetActive(true);
    }

    public void Hide()
    {
        cancellationTokenSource.Cancel();
        transform.gameObject.SetActive(false);
    }

    public override void OnClose()
    {
        cancellationTokenSource.Cancel();
        base.OnClose();
    }
}
