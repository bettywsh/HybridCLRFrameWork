using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class SplashIconPanel : AotPanelBase
{
    public CanvasGroup cngBG;
    public override void OnOpen()
    {
        cngBG.alpha = 0;
        Sequence seq = DOTween.Sequence();
        seq.Append(cngBG.DOFade(1, 1));
        seq.AppendInterval(1);
        seq.Append(cngBG.DOFade(0, 1));
        //动画完成回调
        seq.AppendCallback(() =>
        {
            this.Close();
            AotUIManager.Instance.Open<UpdatePanel>();
        });

    }
}
