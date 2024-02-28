using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPanel : BasePanel
{

    public override async UniTask OnOpen()
    {        
        base.OnOpen();
    }

    [OnClick("btnBag")]
    public void OnClick_Bag()
    {
        UIManager.Instance.Open<BagPanel>();
    }

    [OnClick("btnDialogOne")]
    public void OnClick_DialogOne()
    {
        DialogManager.Instance.ShowDialogOne("", new Vector3(1,2,3).z + "һ����ť", () => {
            Debug.Log("ȷ��");
        });
    }

    [OnClick("btnDialogTwo")]
    public void OnClick_DialogTwo() 
    { 
        DialogManager.Instance.ShowDialogTwo("", "������ť", ()=> {
            Debug.Log("ȷ��");
        }, () =>
        {
            Debug.Log("ȡ��");
        });
    }

    [OnClick("btnFlyingText")]
    public void OnClick_FlyingText() 
    {
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
