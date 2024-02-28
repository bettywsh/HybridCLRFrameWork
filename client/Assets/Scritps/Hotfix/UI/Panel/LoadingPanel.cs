using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : BasePanel
{

    public override async UniTask OnOpen()
    {
		base.OnOpen();
        transform.GetComponent<Canvas>().sortingOrder = (int)EUILayer.Loading;
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
