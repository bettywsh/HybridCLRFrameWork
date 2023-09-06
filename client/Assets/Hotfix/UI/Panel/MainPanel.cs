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
        DialogManager.Instance.ShowDialogOne("", "һ����ť", () => {
            Debug.Log("ȷ��");
        });
    }

    void Click_btn_DialogTwo() 
    { 
        DialogManager.Instance.ShowDialogTwo("", "������ť", ()=> {
            Debug.Log("ȷ��");
        }, () =>
        {
            Debug.Log("ȡ��");
        });
    }

    void Click_btn_FlyingText() {
        DialogManager.Instance.ShowTextFlying("Ư��1");
        DialogManager.Instance.ShowTextFlying("Ư��2");
        DialogManager.Instance.ShowTextFlying("Ư��3");
        DialogManager.Instance.ShowTextFlying("Ư��4");
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
