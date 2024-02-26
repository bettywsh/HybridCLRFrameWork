using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashAdvicePanel : BasePanel
{
    public CanvasGroup cng_Content;


    public override async UniTask OnOpen()
    {
        CanvasGroup cngContent = referenceData["cngContent"].cngValue;
        cngContent.alpha = 0;
        Sequence seq = DOTween.Sequence();
        seq.Append(cngContent.DOFade(1, 1));
        seq.AppendInterval(1);
        seq.Append(cngContent.DOFade(0, 1));
        //动画完成回调
        seq.AppendCallback(() =>
        {
            UIManager.Instance.Close<SplashAdvicePanel>();
            UIManager.Instance.Open<SplashIconPanel>();
        });
    }

    public override void OnClose()
    { 
    
    }
}
