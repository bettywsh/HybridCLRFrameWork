using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashAdvicePanel : AotPanelBase
{
    public CanvasGroup cngContent;

    public override async void OnOpen()
    {
        cngContent.alpha = 0;
        Sequence seq = DOTween.Sequence();
        seq.Append(cngContent.DOFade(1, 1));
        seq.AppendInterval(1);
        seq.Append(cngContent.DOFade(0, 1));
        //动画完成回调
        //seq.AppendCallback(() =>
        //{
        //    this.Close();
        //    AotUIManager.Instance.Open<SplashIconPanel>();
        //});
        await seq;        
        AotUIManager.Instance.Open<SplashIconPanel>();
        this.Close();
        //await transform.DOMoveX(2, 10);
        //await DOTween.To(() => timeCount, a => timeCount = a, 1, 3);
    }
}
