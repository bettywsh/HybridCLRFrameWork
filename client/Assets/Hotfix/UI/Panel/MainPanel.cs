using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : BasePanel
{
    public MainPanelView view;

    public override void OnOpen()
    {
        view = transform.GetComponent<MainPanelView>();
        base.OnOpen();
    }

    void Click_btn_Bag()
    {
        UIManager.Instance.Open<BagPanel>();
    }

    void Click_btn_DialogOne()
    {
        DialogManager.Instance.ShowDialogOne("", "一个按钮", () => {
            Debug.Log("确定");
        });
    }

    void Click_btn_DialogTwo() 
    { 
        DialogManager.Instance.ShowDialogTwo("", "两个按钮", ()=> {
            Debug.Log("确定");
        }, () =>
        {
            Debug.Log("取消");
        });
    }

    void Click_btn_FlyingText() {
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
