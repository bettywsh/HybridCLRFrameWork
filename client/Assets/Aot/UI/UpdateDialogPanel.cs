using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UpdateDialogPanel : AotBasePanel
{
    AotDialogInfo dialogInfo;
    public void OnOpen()
    {
        dialogInfo = args[0] as AotDialogInfo;
        txt_Msg.text = dialogInfo.txtMsg;
        txt_Ok.text = "确定";
        txt_Cancel.text = "取消";
        if (dialogInfo.txtOk != null)
        {
            //txt_Ok.text = TextMgr:GetText(dialogInfo.txtOk);
        }

        if (dialogInfo.txtCal != null) { 
            //txt_Cancel.text = TextMgr:GetText(dialogInfo.txtCal);
        }

        if (dialogInfo.okFun != null)
        {
            btn_Ok.gameObject.SetActive(true);
        }
        if (dialogInfo.calFun != null) {
            btn_Cancel.gameObject.SetActive(true);
        }
    }

    void OnClick_btn_Ok()
    {
        dialogInfo.okFun?.Invoke();
        AotUI.Instance.Close("UpdateDialogPanel");
    }

    void OnClick_btn_Cancel()
    {
        dialogInfo.calFun?.Invoke();
        AotUI.Instance.Close("UpdateDialogPanel");
    }


    public void OnClose()
    {
        
    }

}
