using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AotDialog : AotSingleton<AotDialog>
{
    public void ShowDialog(AotDialogInfo dialogInfo)
    {
        AotUI.Instance.Open("UpdateDialogPanel", dialogInfo);
    }

    public void ShowDialogOne(string txtTitle, string txtMsg, Action okCb)
    {
        AotDialogInfo dialogInfo = new AotDialogInfo();
        dialogInfo.txtTitle = txtTitle;
        dialogInfo.txtMsg = txtMsg;
        dialogInfo.okFun = okCb;
        AotUI.Instance.Open("UpdateDialogPanel", dialogInfo);
    }

    public void ShowDialogTwo(string txtTitle, string txtMsg, Action okFun, Action calFun)
    {
        AotDialogInfo dialogInfo = new AotDialogInfo();
        dialogInfo.txtTitle = txtTitle;
        dialogInfo.txtMsg = txtMsg;
        dialogInfo.okFun = okFun;
        dialogInfo.calFun = calFun;
        AotUI.Instance.Open("UpdateDialogPanel", dialogInfo);
    }
}


public class AotDialogInfo {
    public string txtTitle;
    public string txtMsg;
    public Action okFun;
    public Action calFun;
    public string txtOk;
    public string txtCal;
}