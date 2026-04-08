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
        GetUI("txtTitle").tmptxtValue.text = dialogInfo.txtTitle;
        if (dialogInfo.time > 0)
        {
            GetUI("txtMsg").tmptxtValue.text = string.Format(dialogInfo.txtMsg, dialogInfo.time);
            TimerManager.Instance.Clear(TimerConst.DialogTime);
            TimerManager.Instance.RepeatedTimer(TimerConst.DialogTime, dialogInfo.time, 1);
        }
        else
        {
            GetUI("txtMsg").tmptxtValue.text = dialogInfo.txtMsg;
        }
        GetUI("txtOk").tmptxtValue.text = "确定";
        GetUI("txtCancel").tmptxtValue.text = "取消";
        if (dialogInfo.txtOk != null)
        {
            GetUI("txtOk").tmptxtValue.text = dialogInfo.txtOk;
        }

        if (dialogInfo.txtCal != null)
        {
            GetUI("txtCancel").tmptxtValue.text = dialogInfo.txtCal;
        }

        if (dialogInfo.okFun != null)
        {
            GetUI("btnOk").btnValue.gameObject.SetActive(true);
        }
        else
        {
            GetUI("btnOk").btnValue.gameObject.SetActive(false);
        }
        if (dialogInfo.calFun != null)
        {
            GetUI("btnCancel").btnValue.gameObject.SetActive(true);
        }
        else
        {
            GetUI("btnCancel").btnValue.gameObject.SetActive(false);
        }
    }

    [OnTimer(TimerConst.DialogTime)]
    void OnTimerDialogTime(int time)
    {
        GetUI("txtMsg").tmptxtValue.text = string.Format(dialogInfo.txtMsg, time);
    }

    [OnClick("btnOk")]
    public void OnClick_btnOk()
    {
        this.Close();
        dialogInfo.okFun?.Invoke();
    }

    [OnClick("btnCancel")]
    public void OnClick_btnCancel()
    {
        this.Close();
        dialogInfo.calFun?.Invoke();        
    }

    public override void OnClose()
    {
        if (dialogInfo.time > 0)
        {
            TimerManager.Instance.Clear(TimerConst.DialogTime);
        }
        base.OnClose();
    }
}
