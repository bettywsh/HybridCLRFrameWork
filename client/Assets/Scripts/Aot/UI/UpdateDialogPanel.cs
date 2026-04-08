using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;

public class UpdateDialogPanel : AotPanelBase
{
    public TextMeshProUGUI txtTitle;
    public TextMeshProUGUI txtMsg;
    public TextMeshProUGUI txtOk;
    public TextMeshProUGUI txtCancel;
    public Button btnOk;
    public Button btnCancel;

    AotDialogInfo dialogInfo;
    public override void OnOpen()
    {
        dialogInfo = args[0] as AotDialogInfo;
        txtMsg.text = dialogInfo.txtMsg;
        txtOk.text = "确定";
        txtCancel.text = "取消";
        if (dialogInfo.txtOk != null)
        {
            txtOk.text = dialogInfo.txtOk;
        }

        if (dialogInfo.txtCal != null)
        {
            txtCancel.text = dialogInfo.txtCal;
        }

        if (dialogInfo.okFun != null)
        {
            btnOk.gameObject.SetActive(true);
        }
        else 
        {
            btnOk.gameObject.SetActive(false);
        }
        if (dialogInfo.calFun != null)
        {
            btnCancel.gameObject.SetActive(true);
        }
        else
        {
            btnCancel.gameObject.SetActive(false);
        }
        btnOk.onClick.AddListener(OnClick_btnOk);
        btnCancel.onClick.AddListener(OnClick_btnCancel);
    }

    void OnClick_btnOk()
    {
        this.Close();
        dialogInfo.okFun?.Invoke();        
    }

    void OnClick_btnCancel()
    {
        this.Close();
        dialogInfo.calFun?.Invoke();        
    }



}
