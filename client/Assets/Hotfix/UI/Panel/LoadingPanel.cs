using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LoadingPanel : BasePanel
{
	public LoadingPanelView view;
	
    public override void OnOpen()
    {		
        view = transform.GetComponent<LoadingPanelView>();
		base.OnOpen();
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
