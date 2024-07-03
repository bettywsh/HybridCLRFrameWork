using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : PanelBase
{

    public override async UniTask OnOpen()
    {
		await base.OnOpen();
        transform.GetComponent<Canvas>().sortingOrder = (int)EUILayer.Loading;
    }

    [OnMessage(MessageConst.Msg_LoadingPanelProgress)]
    void OnSetProgress(float progress)
    { 
    
    }

    [OnMessage(MessageConst.Msg_LoadingPanelComplete)]
    void OnSetComplete()
    {
        this.Close();
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
