using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPanel : BasePanel
{
    DialogInfo dialogInfo;

    public override async UniTask OnOpen()
    {        
        base.OnOpen();
        dialogInfo = args[0] as DialogInfo;
        transform.GetComponent<Canvas>().sortingOrder = (int)dialogInfo.layer;
        referenceData["txtMsg"].tmptxtValue.text = dialogInfo.txtMsg;
        referenceData["txtOk"].tmptxtValue.text = "确定";
        referenceData["txtCancel"].tmptxtValue.text = "取消";
        if (dialogInfo.txtOk != null)
        {
            referenceData["txtOk"].tmptxtValue.text = "确定";
        }

        if (dialogInfo.txtCal != null)
        {
            referenceData["txtCancel"].tmptxtValue.text = "取消";
        }

        if (dialogInfo.okFun != null)
        {
            referenceData["btnCancel"].btnValue.gameObject.SetActive(true);
        }
        if (dialogInfo.calFun != null)
        {
            referenceData["btnOk"].btnValue.gameObject.SetActive(true);
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
