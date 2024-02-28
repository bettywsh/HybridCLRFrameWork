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
        DialogManager.Instance.ShowDialogOne("", new Vector3(1,2,3).z + "一个按钮", () => {
            Debug.Log("确定");
        });
    }

    [OnClick("btnDialogTwo")]
    public void OnClick_DialogTwo() 
    { 
        DialogManager.Instance.ShowDialogTwo("", "两个按钮", ()=> {
            Debug.Log("确定");
        }, () =>
        {
            Debug.Log("取消");
        });
    }

    [OnClick("btnFlyingText")]
    public void OnClick_FlyingText() 
    {
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
