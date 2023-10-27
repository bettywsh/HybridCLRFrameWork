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

    void OnClick_Bag()
    {
        UIManager.Instance.Open<BagPanel>();
    }

    void OnClick_DialogOne()
    {
        DialogManager.Instance.ShowDialogOne("", new Vector3(1,2,3).z + "һ����ť", () => {
            Debug.Log("ȷ��");
        });
    }

    void OnClick_DialogTwo() 
    { 
        DialogManager.Instance.ShowDialogTwo("", "������ť", ()=> {
            Debug.Log("ȷ��");
        }, () =>
        {
            Debug.Log("ȡ��");
        });
    }

    void OnClick_FlyingText() {
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
