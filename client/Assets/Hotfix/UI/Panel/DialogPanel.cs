using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogPanel : BasePanel
{
	public DialogPanelView view;
    DialogInfo dialogInfo;

    public override void OnBindEvent()
    {
        view = transform.GetComponent<DialogPanelView>();
        base.OnBindEvent();
    }

    public override void OnOpen()
    {        
        base.OnOpen();

        dialogInfo = args[0] as DialogInfo;
        view.txt_Msg.text = dialogInfo.txtMsg;
        view.txt_Ok.text = "确定";
        view.txt_Cancel.text = "取消";
        if (dialogInfo.txtOk != null)
        {
            view.txt_Ok.text = "确定";
        }

        if (dialogInfo.txtCal != null)
        {
            view.txt_Cancel.text = "取消";
        }

        if (dialogInfo.okFun != null)
        {
            view.btn_Ok.gameObject.SetActive(true);
        }
        if (dialogInfo.calFun != null)
        {
            view.btn_Cancel.gameObject.SetActive(true);
        }
    }

    void Click_btn_Ok()
    {
        dialogInfo.okFun?.Invoke();
        UIManager.Instance.Close<DialogPanel>();
    }

    void Click_btn_Cancel()
    {
        dialogInfo.calFun?.Invoke();
        UIManager.Instance.Close<DialogPanel>();
    }

    public override void OnClose()
    {
        
    }
}
