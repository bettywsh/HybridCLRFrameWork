using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReConnectLoadingPanel : PanelBase
{
    public override void OnBindEvent()
    {
        base.OnBindEvent();
    }
    public override async UniTask OnOpen()
    { 
        await base.OnOpen();
        transform.GetComponent<Canvas>().sortingOrder = (int)EUILayer.ReConnectLoadingPanel;
    }

    [OnMessage(MessageConst.Msg_ReConnectPanelClose)]
    void OnMessage()
    { 
        this.Close();
    }

    public override void OnUnBindEvent()
    { 
        base.OnUnBindEvent();
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
