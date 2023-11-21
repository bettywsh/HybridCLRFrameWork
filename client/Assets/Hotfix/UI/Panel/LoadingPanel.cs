using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : BasePanel
{

    public override void OnBindEvent()
    {
        transform.GetComponent<Canvas>().sortingOrder = (int)EUILayer.Loading;
        base.OnBindEvent();
    }

    public override void OnOpen()
    {
		base.OnOpen();
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
