using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UpdatePanel : BasePanel
{
    public void OnOpen()
    {
        MessageManager.Instance.RegisterEventMessageHandler(MessageConst.MsgUpdateFristCopy, Msg_UpdateFristCopy);
        MessageManager.Instance.RegisterEventMessageHandler(MessageConst.MsgUpdateFristProgress, Msg_UpdateFristProgress);
        MessageManager.Instance.RegisterEventMessageHandler(MessageConst.MsgUpdateCheckVersion, Msg_UpdateCheckVersion);
        MessageManager.Instance.RegisterEventMessageHandler(MessageConst.MsgUpdateBigVersion, Msg_UpdateBigVersion);
        MessageManager.Instance.RegisterEventMessageHandler(MessageConst.MsgUpdateSmallVersion, Msg_UpdateSmallVersion);
        MessageManager.Instance.RegisterEventMessageHandler(MessageConst.MsgUpdateLostConnect, Msg_UpdateLostConnect);
    }


    public void OnClose()
    {
        
    }

    private void Msg_UpdateFristCopy(object[] msgDatas)
    {
        txt_Content.text = "��ѹ�С�����";
    }

    private void Msg_UpdateFristProgress(object[] msgDatas)
    {
        img_cur.fillAmount = ((int)msgDatas[0] + 1) / (int)msgDatas[1];
    }

    private void Msg_UpdateCheckVersion(object[] msgDatas)
    {
        txt_Content.text = "�汾���";
    }

    private void Msg_UpdateBigVersion(object[] msgDatas)
    {
        DialogManager.Instance.ShowDialogOne("����", "��汾����������www.baidu.com", () => { 
            Application.Quit();
        });
    }

    private void Msg_UpdateSmallVersion(object[] msgDatas)
    {
        DialogManager.Instance.ShowDialogTwo("����", "С�汾����" + msgDatas[0], () => {
            UpdateManager.Instance.DownLoadFiles();
        }, () =>{
            Application.Quit();
        });
    }
    private void Msg_UpdateLostConnect(object[] msgDatas)
    {
        DialogManager.Instance.ShowDialogOne("����", "ʧȥ����", () => {
            UpdateManager.Instance.CheckVersion();
        });
    }
}
