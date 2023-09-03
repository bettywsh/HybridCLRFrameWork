using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SplashAdvicePanel : AotBasePanel
{


    public void OnOpen()
    {
        cng_Content.alpha = 0;
        Sequence seq = DOTween.Sequence();
        seq.Append(cng_Content.DOFade(1, 1));
        seq.AppendInterval(1);
        seq.Append(cng_Content.DOFade(0, 1));
        //动画完成回调
        seq.AppendCallback(() =>
        {
            AotUI.Instance.Close("SplashAdvicePanel");
            AotUI.Instance.Open("SplashIconPanel");
        });
    }

    public void OnClose()
    { 
    
    }
}
