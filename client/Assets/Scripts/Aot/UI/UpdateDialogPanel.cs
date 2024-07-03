using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

public class UpdateDialogPanel : AotPanelBase
{
    public TextMeshProUGUI txtMsg;
    public TextMeshProUGUI txtOk;
    public TextMeshProUGUI txtCancel;
    public Button btnOk;
    public Button btnCancel;

    AotDialogInfo dialogInfo;
    private void Awake()
    {
        dialogInfo = args[0] as AotDialogInfo;
        txtMsg.text = dialogInfo.txtMsg;
        txtOk.text = "确定";
        txtCancel.text = "取消";
        if (dialogInfo.txtOk != null)
        {
            //txt_Ok.text = TextMgr:GetText(dialogInfo.txtOk);
        }

        if (dialogInfo.txtCal != null)
        {
            //txt_Cancel.text = TextMgr:GetText(dialogInfo.txtCal);
        }

        if (dialogInfo.okFun != null)
        {
            btnOk.gameObject.SetActive(true);
        }
        if (dialogInfo.calFun != null)
        {
            btnCancel.gameObject.SetActive(true);
        }
        btnOk.onClick.AddListener(OnClick_btnOk);
        btnCancel.onClick.AddListener(OnClick_btnCancel);
    }

    void OnClick_btnOk()
    {
        dialogInfo.okFun?.Invoke();
        this.Close();
    }

    void OnClick_btnCancel()
    {
        dialogInfo.calFun?.Invoke();
        this.Close();
    }



}
