using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AotDialogManager : AotSingleton<AotDialogManager>
{
    public async UniTask ShowDialog(AotDialogInfo dialogInfo)
    {
        await AotUIManager.Instance.Open<UpdateDialogPanel>(dialogInfo);
    }

    public async UniTask ShowDialogOne(string txtTitle, string txtMsg, Action okCb)
    {
        AotDialogInfo dialogInfo = new AotDialogInfo();
        dialogInfo.txtTitle = txtTitle;
        dialogInfo.txtMsg = txtMsg;
        dialogInfo.okFun = okCb;
        await AotUIManager.Instance.Open<UpdateDialogPanel>(dialogInfo);
    }

    public async UniTask ShowDialogTwo(string txtTitle, string txtMsg, Action okFun, Action calFun)
    {
        AotDialogInfo dialogInfo = new AotDialogInfo();
        dialogInfo.txtTitle = txtTitle;
        dialogInfo.txtMsg = txtMsg;
        dialogInfo.okFun = okFun;
        dialogInfo.calFun = calFun;
        await AotUIManager.Instance.Open<UpdateDialogPanel>(dialogInfo);
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