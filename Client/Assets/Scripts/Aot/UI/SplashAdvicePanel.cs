using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashAdvicePanel : AotPanelBase
{
    public CanvasGroup cngContent;

    public override async UniTask OnOpen()
    {
        cngContent.alpha = 0;
        Sequence seq = DOTween.Sequence();
        await seq.Append(cngContent.DOFade(1, 1))
            .AppendInterval(1)
            .Append(cngContent.DOFade(0, 1));
        await UniTask.SwitchToMainThread();
        AotUIManager.Instance.Open<UpdatePanel>().Forget();
        this.Close();
        //await transform.DOMoveX(2, 10);
        //await DOTween.To(() => timeCount, a => timeCount = a, 1, 3);
    }
}
