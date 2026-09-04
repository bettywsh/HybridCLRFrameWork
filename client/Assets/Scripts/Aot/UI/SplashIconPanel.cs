using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class SplashIconPanel : AotPanelBase
{
    public CanvasGroup cngBG;
    public override async UniTask OnOpen()
    {
        cngBG.alpha = 0;
        Sequence seq = DOTween.Sequence();
        await seq.Append(cngBG.DOFade(1, 1))
            .AppendInterval(1)
            .Append(cngBG.DOFade(0, 1));
        await UniTask.SwitchToMainThread();
        AotUIManager.Instance.Open<UpdatePanel>().Forget();
        this.Close();
    }

    
}
