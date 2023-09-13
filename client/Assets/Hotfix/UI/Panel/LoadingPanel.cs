using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPanel : BasePanel
{
	public LoadingPanelView view;

    public override void OnBindEvent()
    {
        view = transform.GetComponent<LoadingPanelView>();
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
