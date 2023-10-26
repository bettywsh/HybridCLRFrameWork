using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : BasePanel
{

    public override void OnBindEvent()
    {
        base.OnBindEvent();
    }

    public override void OnOpen()
    {        
        base.OnOpen();
    }

    void Click_Bag()
    {
        UIManager.Instance.Open<BagPanel>();
    }

    void Click_DialogOne()
    {
        DialogManager.Instance.ShowDialogOne("", new Vector3(1,2,3).z + "一个按钮", () => {
            Debug.Log("确定");
        });
    }

    void Click_DialogTwo() 
    { 
        DialogManager.Instance.ShowDialogTwo("", "两个按钮", ()=> {
            Debug.Log("确定");
        }, () =>
        {
            Debug.Log("取消");
        });
    }

    void Click_FlyingText() {
        DialogManager.Instance.ShowTextFlying("漂字1");
        DialogManager.Instance.ShowTextFlying("漂字2");
        DialogManager.Instance.ShowTextFlying("漂字3");
        DialogManager.Instance.ShowTextFlying("漂字4");
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
