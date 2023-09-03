using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UpdatePanel : AotBasePanel
{
    public void OnOpen()
    {
        
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
        AotDialog.Instance.ShowDialogOne("����", "��汾����������www.baidu.com", () => { 
            Application.Quit();
        });
    }

    private void Msg_UpdateSmallVersion(object[] msgDatas)
    {
        AotDialog.Instance.ShowDialogTwo("����", "С�汾����" + msgDatas[0], () => {
            AotUpdate.Instance.DownLoadFiles();
        }, () =>{
            Application.Quit();
        });
    }
    private void Msg_UpdateLostConnect(object[] msgDatas)
    {
        AotDialog.Instance.ShowDialogOne("����", "ʧȥ����", () => {
            AotUpdate.Instance.CheckVersion();
        });
    }
}
