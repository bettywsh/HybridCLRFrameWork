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
        GetUI("txtMsg").tmptxtValue.text = dialogInfo.txtMsg;
        GetUI("txtOk").tmptxtValue.text = "确定";
        GetUI("txtCancel").tmptxtValue.text = "取消";
        if (dialogInfo.txtOk != null)
        {
            GetUI("txtOk").tmptxtValue.text = "确定";
        }

        if (dialogInfo.txtCal != null)
        {
            GetUI("txtCancel").tmptxtValue.text = "取消";
        }

        if (dialogInfo.okFun != null)
        {
            GetUI("btnCancel").btnValue.gameObject.SetActive(true);
        }
        if (dialogInfo.calFun != null)
        {
            GetUI("btnOk").btnValue.gameObject.SetActive(true);
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
