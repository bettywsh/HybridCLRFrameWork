using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : Singleton<DialogManager>
{    

    public override void Init()
    {
        EventHelper.RegisterTimerEvent(this);
    }

    #region 飘字
    public async UniTask ShowTextFlying(string value)
    {
        TextPanel msgPanel = UIManager.Instance.GetUI<TextPanel>();
        msgPanel ??= await UIManager.Instance.Open<TextPanel>(value);
        msgPanel.Fly(value);
    }

    public async UniTask ShowDialog(DialogInfo dialogInfo)
    {
        await UIManager.Instance.Open<DialogPanel>(dialogInfo);
    }
    #endregion

    #region 网络菊花
    public async UniTask ShowNetLoading(float timeout)
    {
        NetLoadingPanel netLoadingPanel = UIManager.Instance.GetUI<NetLoadingPanel>();
        netLoadingPanel ??= await UIManager.Instance.Open<NetLoadingPanel>();
        netLoadingPanel.Show().Forget();
        TimerManager.Instance.Clear(TimerConst.NetLoading);
        TimerManager.Instance.OnceTimer(TimerConst.NetLoading, timeout);
    }

    public async UniTask ShowNetLoading()
    {
        NetLoadingPanel netLoadingPanel = UIManager.Instance.GetUI<NetLoadingPanel>();
        netLoadingPanel ??= await UIManager.Instance.Open<NetLoadingPanel>();
        netLoadingPanel.Show().Forget();
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
        DialogInfo dialogInfo = new()
        {
            layer = EUILayer.Dialog,
            txtTitle = txtTitle,
            txtMsg = txtMsg,
            okFun = okCb
        };
        UIManager.Instance.Open<DialogPanel>(dialogInfo).Forget();
    }

    public void ShowDialogOne(string txtTitle, string txtMsg, string txtOk, Action okCb)
    {
        DialogInfo dialogInfo = new()
        {
            layer = EUILayer.Dialog,
            txtTitle = txtTitle,
            txtMsg = txtMsg,
            okFun = okCb,
            txtOk = txtOk
        };
        UIManager.Instance.Open<DialogPanel>(dialogInfo).Forget();
    }

    public void ShowDialogTwo(string txtTitle, string txtMsg, Action okFun, Action calFun)
    {
        DialogInfo dialogInfo = new()
        {
            layer = EUILayer.Dialog,
            txtTitle = txtTitle,
            txtMsg = txtMsg,
            okFun = okFun,
            calFun = calFun
        };
        UIManager.Instance.Open<DialogPanel>(dialogInfo).Forget();
    }

    public void ShowDialogTwo(string txtTitle, string txtMsg, string txtOk, string txtCal, Action okFun, Action calFun)
    {
        DialogInfo dialogInfo = new()
        {
            layer = EUILayer.Dialog,
            txtTitle = txtTitle,
            txtMsg = txtMsg,
            txtOk = txtOk,
            txtCal = txtCal,
            okFun = okFun,
            calFun = calFun
        };
        UIManager.Instance.Open<DialogPanel>(dialogInfo).Forget();
    }
    #endregion

    #region 系统级别层级高于新手引导

    public void ShowSystemDialogOne(string txtTitle, string txtMsg, Action okCb)
    {
        DialogInfo dialogInfo = new()
        {
            layer = EUILayer.DialogSystem,
            txtTitle = txtTitle,
            txtMsg = txtMsg,
            okFun = okCb
        };
        UIManager.Instance.Open<DialogSystemPanel>(dialogInfo).Forget();
    }

    public void ShowSystemDialogTwo(string txtTitle, string txtMsg, Action okFun, Action calFun)
    {
        DialogInfo dialogInfo = new()
        {
            layer = EUILayer.DialogSystem,
            txtTitle = txtTitle,
            txtMsg = txtMsg,
            okFun = okFun,
            calFun = calFun
        };
        UIManager.Instance.Open<DialogSystemPanel>(dialogInfo).Forget();
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