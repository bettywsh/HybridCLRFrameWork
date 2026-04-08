using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : PanelBase
{

    public override async UniTask OnOpen()
    {
		await base.OnOpen();
        transform.GetComponent<Canvas>().sortingOrder = (int)EUILayer.Loading;
        GetUI("Fill").imgValue.fillAmount = 0;
        GetUI("Fill").imgValue.DOFillAmount(1f, 3f).SetUpdate(true).SetEase(Ease.Linear).OnComplete(() => { 
            this.Close();
        });
        //Random.Range

        //GetUI("Tips").tmptxtValue.text = "";
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
