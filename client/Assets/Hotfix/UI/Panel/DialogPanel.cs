using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DialogPanel : BasePanel
{
    DialogInfo dialogInfo;
    public void OnOpen()
    {
        dialogInfo = args[0] as DialogInfo;
        txt_Msg.text = dialogInfo.txtMsg;

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
        UIManager.Instance.Close("DialogPanel");
    }

    void OnClick_btn_Cancel()
    {
        dialogInfo.calFun?.Invoke();
        UIManager.Instance.Close("DialogPanel");
    }


    public void OnClose()
    {
        
    }

}
