using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPanel : PanelBase
{
    DialogInfo dialogInfo;

    public override async UniTask OnOpen()
    {        
        await base.OnOpen();
        dialogInfo = args[0] as DialogInfo;
        transform.GetComponent<Canvas>().sortingOrder = (int)dialogInfo.layer;
        referenceCollector.Get("txtMsg").tmptxtValue.text = dialogInfo.txtMsg;
        referenceCollector.Get("txtOk").tmptxtValue.text = "确定";
        referenceCollector.Get("txtCancel").tmptxtValue.text = "取消";
        if (dialogInfo.txtOk != null)
        {
            referenceCollector.Get("txtOk").tmptxtValue.text = "确定";
        }

        if (dialogInfo.txtCal != null)
        {
            referenceCollector.Get("txtCancel").tmptxtValue.text = "取消";
        }

        if (dialogInfo.okFun != null)
        {
            referenceCollector.Get("btnCancel").btnValue.gameObject.SetActive(true);
        }
        if (dialogInfo.calFun != null)
        {
            referenceCollector.Get("btnOk").btnValue.gameObject.SetActive(true);
        }
    }

    [OnClick("btnOk")]
    public void OnClick_btnOk()
    {
        dialogInfo.okFun?.Invoke();
        this.Close();
    }

    [OnClick("btnCancel")]
    public void OnClick_btnCancel()
    {
        dialogInfo.calFun?.Invoke();
        this.Close();
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
