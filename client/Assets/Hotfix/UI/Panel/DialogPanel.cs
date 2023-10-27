using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPanel : BasePanel
{
    DialogInfo dialogInfo;

    public override void OnBindEvent()
    {
        base.OnBindEvent();
    }

    public override void OnOpen()
    {        
        base.OnOpen();

        dialogInfo = args[0] as DialogInfo;
        referenceData["txtMsg"].tmptxtValue.text = dialogInfo.txtMsg;
        referenceData["txtOk"].tmptxtValue.text = "ȷ��";
        referenceData["txtCancel"].tmptxtValue.text = "ȡ��";
        if (dialogInfo.txtOk != null)
        {
            referenceData["txtOk"].tmptxtValue.text = "ȷ��";
        }

        if (dialogInfo.txtCal != null)
        {
            referenceData["txtCancel"].tmptxtValue.text = "ȡ��";
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

    void OnClick_btnOk()
    {
        dialogInfo.okFun?.Invoke();
        UIManager.Instance.Close<DialogPanel>();
    }

    void OnClick_btnCancel()
    {
        dialogInfo.calFun?.Invoke();
        UIManager.Instance.Close<DialogPanel>();
    }

    public override void OnClose()
    {
        
    }
}
