using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public partial class SplashIconPanel : BasePanel
{

    public void OnOpen()
    {
        cng_BG.alpha = 0;
        Sequence seq = DOTween.Sequence();
        seq.Append(cng_BG.DOFade(1, 1));
        seq.AppendInterval(1);
        seq.Append(cng_BG.DOFade(0, 1));
        //动画完成回调
        seq.AppendCallback(() =>
        {
            UIManager.Instance.Close("SplashIconPanel");            
        });

    }

    public void OnClose()
    {
        UpdateManager.Instance.CheckVersion();
    }

}
