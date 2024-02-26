using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : Singleton<DialogManager>
{
    TextPanel msgPanel;
    public void ShowTextFlying(string value)
    {
        if (msgPanel == null)
        {
            msgPanel = UIManager.Instance.Open<TextPanel>(value);
        }
        Debug.LogError(value);
        msgPanel.Fly(value);
    }

    public void ShowDialog(DialogInfo dialogInfo)
    {
        UIManager.Instance.Open<DialogPanel>(dialogInfo);
    }

    #region ��ͨ����

    public void ShowDialogOne(string txtTitle, string txtMsg, Action okCb)
    {
        DialogInfo dialogInfo = new DialogInfo();
        dialogInfo.layer = EUILayer.Dialog;
        dialogInfo.txtTitle = txtTitle;
        dialogInfo.txtMsg = txtMsg;
        dialogInfo.okFun = okCb;
        UIManager.Instance.Open<DialogPanel>(dialogInfo);
    }

    public void ShowDialogTwo(string txtTitle, string txtMsg, Action okFun, Action calFun)
    {
        DialogInfo dialogInfo = new DialogInfo();
        dialogInfo.layer = EUILayer.Dialog;
        dialogInfo.txtTitle = txtTitle;
        dialogInfo.txtMsg = txtMsg;
        dialogInfo.okFun = okFun;
        dialogInfo.calFun = calFun;
        UIManager.Instance.Open<DialogPanel>(dialogInfo);
    }
    #endregion

    #region ϵͳ����㼶������������

    public void ShowSystemDialogOne(string txtTitle, string txtMsg, Action okCb)
    {
        DialogInfo dialogInfo = new DialogInfo();
        dialogInfo.layer = EUILayer.DialogSystem;
        dialogInfo.txtTitle = txtTitle;
        dialogInfo.txtMsg = txtMsg;
        dialogInfo.okFun = okCb;
        UIManager.Instance.Open<DialogPanel>(dialogInfo);
    }

    public void ShowSystemDialogTwo(string txtTitle, string txtMsg, Action okFun, Action calFun)
    {
        DialogInfo dialogInfo = new DialogInfo();
        dialogInfo.layer = EUILayer.DialogSystem;
        dialogInfo.txtTitle = txtTitle;
        dialogInfo.txtMsg = txtMsg;
        dialogInfo.okFun = okFun;
        dialogInfo.calFun = calFun;
        UIManager.Instance.Open<DialogPanel>(dialogInfo);
    }
    #endregion
}


public class DialogInfo {
    public EUILayer layer;
    public string txtTitle;
    public string txtMsg;
    public Action okFun;
    public Action calFun;
    public string txtOk;
    public string txtCal;
}