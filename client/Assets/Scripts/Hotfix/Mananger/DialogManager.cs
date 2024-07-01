using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : Singleton<DialogManager>
{
    TextPanel msgPanel;
    NetLoadingPanel netLoadingPanel;

    #region 飘字
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
    #endregion

    #region 网络菊花
    public void ShowNetLoading(float timeout)
    {
        if (netLoadingPanel == null)
        {
            netLoadingPanel = UIManager.Instance.Open<NetLoadingPanel>();
        }
        TimerManager.Instance.ClearTimer("net");
        TimerManager.Instance.SetTimer("net", timeout, () =>
        {
            HideNetLoading();
        });
    }

    public void HideNetLoading()
    {
        TimerManager.Instance.ClearTimer("net");
        netLoadingPanel.Close();
        netLoadingPanel = null;
    }
    #endregion

    #region 普通级别

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

    #region 系统级别层级高于新手引导

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