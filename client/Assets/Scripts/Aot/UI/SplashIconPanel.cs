using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class SplashIconPanel : PanelBase
{
    public CanvasGroup cng_BG;
    public override async UniTask OnOpen()
    {
        CanvasGroup cngBG = referenceData["cngBG"].cngValue;
        cngBG.alpha = 0;
        Sequence seq = DOTween.Sequence();
        seq.Append(cngBG.DOFade(1, 1));
        seq.AppendInterval(1);
        seq.Append(cngBG.DOFade(0, 1));
        //动画完成回调
        seq.AppendCallback(() =>
        {
            this.Close();
            UIManager.Instance.Open<UpdatePanel>();
        });

    }

    public override void OnClose()
    {

    }

}
