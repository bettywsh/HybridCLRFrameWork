using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : Singleton<DialogManager>
{    

    public override async UniTask Init()
    {
        await base.Init();
        EventHelper.RegisterTimerEvent(this);
    }

    #region 飘字
    public async void ShowTextFlying(string value)
    {
        TextPanel msgPanel = UIManager.Instance.GetUI<TextPanel>();
        if (msgPanel == null)
        {
            msgPanel = await UIManager.Instance.Open<TextPanel>(value);
        }
        msgPanel.Fly(value);
    }

    public async void ShowDialog(DialogInfo dialogInfo)
    {
        await UIManager.Instance.Open<DialogPanel>(dialogInfo);
    }
    #endregion

    #region 网络菊花
    public async void ShowNetLoading(float timeout)
    {
        NetLoadingPanel netLoadingPanel = UIManager.Instance.GetUI<NetLoadingPanel>();
        if (netLoadingPanel == null)
        {
            netLoadingPanel = await UIManager.Instance.Open<NetLoadingPanel>();
        }
        TimerManager.Instance.Clear(TimerConst.NetLoading);
        TimerManager.Instance.OnceTimer(TimerConst.NetLoading, timeout);
    }

    public async void ShowNetLoading()
    {
        NetLoadingPanel netLoadingPanel = UIManager.Instance.GetUI<NetLoadingPanel>();
        if (netLoadingPanel == null)
        {
            netLoadingPanel = await UIManager.Instance.Open<NetLoadingPanel>();
        }
        netLoadingPanel.Show();
    }

    [OnTimer(TimerConst.NetLoading)]
    public void HideNetLoading()
    {
        NetLoadingPanel netLoadingPanel = UIManager.Instance.GetUI<NetLoadingPanel>();
        netLoadingPanel?.Hide();
        //netLoadingPanel?.Close();
        //netLoadingPanel = null;
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

    public void ShowDialogOne(string txtTitle, string txtMsg, string txtOk, Action okCb)
    {
        DialogInfo dialogInfo = new DialogInfo();
        dialogInfo.layer = EUILayer.Dialog;
        dialogInfo.txtTitle = txtTitle;
        dialogInfo.txtMsg = txtMsg;
        dialogInfo.okFun = okCb;
        dialogInfo.txtOk = txtOk;
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

    public void ShowDialogTwo(string txtTitle, string txtMsg, string txtOk, string txtCal, Action okFun, Action calFun)
    {
        DialogInfo dialogInfo = new DialogInfo();
        dialogInfo.layer = EUILayer.Dialog;
        dialogInfo.txtTitle = txtTitle;
        dialogInfo.txtMsg = txtMsg;
        dialogInfo.txtOk = txtOk;
        dialogInfo.txtCal = txtCal;
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
        UIManager.Instance.Open<DialogSystemPanel>(dialogInfo);
    }

    public void ShowSystemDialogTwo(string txtTitle, string txtMsg, Action okFun, Action calFun)
    {
        DialogInfo dialogInfo = new DialogInfo();
        dialogInfo.layer = EUILayer.DialogSystem;
        dialogInfo.txtTitle = txtTitle;
        dialogInfo.txtMsg = txtMsg;
        dialogInfo.okFun = okFun;
        dialogInfo.calFun = calFun;
        UIManager.Instance.Open<DialogSystemPanel>(dialogInfo);
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
    public int time;
}