using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : Singleton<DialogManager>
{
    TextFlyingPanel textFlyingPanel;
    public async void ShowTextFlying(string value)
    {
        if (textFlyingPanel == null)
        {
            textFlyingPanel = await UIManager.Instance.Open<TextFlyingPanel>(value);
        }
        textFlyingPanel.Fly(value);
    }

    public void ShowDialog(DialogInfo dialogInfo)
    {
        UIManager.Instance.Open<DialogPanel>(dialogInfo);
    }

    public void ShowDialogOne(string txtTitle, string txtMsg, Action okCb)
    {
        DialogInfo dialogInfo = new DialogInfo();
        dialogInfo.txtTitle = txtTitle;
        dialogInfo.txtMsg = txtMsg;
        dialogInfo.okFun = okCb;
        UIManager.Instance.Open<DialogPanel>(dialogInfo);
    }

    public void ShowDialogTwo(string txtTitle, string txtMsg, Action okFun, Action calFun)
    {
        DialogInfo dialogInfo = new DialogInfo();
        dialogInfo.txtTitle = txtTitle;
        dialogInfo.txtMsg = txtMsg;
        dialogInfo.okFun = okFun;
        dialogInfo.calFun = calFun;
        UIManager.Instance.Open<DialogPanel>(dialogInfo);
    }
}


public class DialogInfo {
    public string txtTitle;
    public string txtMsg;
    public Action okFun;
    public Action calFun;
    public string txtOk;
    public string txtCal;
}