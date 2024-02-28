using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

public class UpdateDialogPanel : BasePanel
{
    AotDialogInfo dialogInfo;
    public override async UniTask OnOpen()
    {
        dialogInfo = args[0] as AotDialogInfo;
        referenceData["txtMsg"].tmptxtValue.text = dialogInfo.txtMsg;
        referenceData["txtOk"].tmptxtValue.text = "确定";
        referenceData["txtCancel"].tmptxtValue.text = "取消";
        if (dialogInfo.txtOk != null)
        {
            //txt_Ok.text = TextMgr:GetText(dialogInfo.txtOk);
        }

        if (dialogInfo.txtCal != null) { 
            //txt_Cancel.text = TextMgr:GetText(dialogInfo.txtCal);
        }

        if (dialogInfo.okFun != null)
        {
            referenceData["btnOk"].btnValue.gameObject.SetActive(true);
        }
        if (dialogInfo.calFun != null) {
            referenceData["btnCancel"].btnValue.gameObject.SetActive(true);
        }
    }

    void OnClick_btnOk()
    {
        dialogInfo.okFun?.Invoke();
        this.Close();
    }

    void OnClick_btnCancel()
    {
        dialogInfo.calFun?.Invoke();
        this.Close();
    }


    public override void OnClose()
    {
        
    }

}
