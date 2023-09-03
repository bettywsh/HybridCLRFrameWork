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
        txt_Ok.text = "确定";
        txt_Cancel.text = "取消";
        if (dialogInfo.txtOk != null)
        {
            txt_Ok.text = "确定";
        }

        if (dialogInfo.txtCal != null)
        {
            txt_Cancel.text = "取消";
        }

        if (dialogInfo.okFun != null)
        {
            btn_Ok.gameObject.SetActive(true);
        }
        if (dialogInfo.calFun != null)
        {
            btn_Cancel.gameObject.SetActive(true);
        }
    }

    void Click_btn_Ok()
    {
        dialogInfo.okFun?.Invoke();
        UIManager.Instance.Close("DialogPanel");
    }

    void Click_btn_Cancel()
    {
        dialogInfo.calFun?.Invoke();
        UIManager.Instance.Close("DialogPanel");
    }

    public void OnClose()
    {
        
    }

}
