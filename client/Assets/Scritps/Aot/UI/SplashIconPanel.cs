using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SplashIconPanel : BasePanel
{
    public CanvasGroup cng_BG;
    public override void OnOpen()
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
            UIManager.Instance.Close<SplashIconPanel>();
            UIManager.Instance.Open<UpdatePanel>();
        });

    }

    public override void OnClose()
    {

    }

}
