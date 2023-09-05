using com.bochsler.protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LoginPanel : BasePanel
{
	public LoginPanelView view;
	
    public void OnOpen()
    {
        base.OnOpen();
        view = transform.GetComponent<LoginPanelView>();

        DialogManager.Instance.ShowDialogOne("警告", "进入游戏www.baidu.com", () => {
            Application.Quit();
        });
    }

    void Msg_Connected(object[] msgDatas)
    {
        Debug.LogError("连接成功");
        LoginRequest lr = new LoginRequest();
        lr.sceneType = SceneType.SceneTypeGALLERY;
        lr.Password = "";
        lr.Username = "6422bf99fad65bd0a84c10ab";
        lr.loginType = 0;
        NetworkManager.Instance.Send(CSMessageEnum.LoginRequest, lr);
    }

    void Net_LoginResponse(object[] msgDatas)
    {
        Debug.LogError("LoginResponse");
    }

    void Click_btn_Ok()
    {
        //List<HorseConfigItem> list = ConfigManager.Instance.LoadConfig<HorseConfig>().GetAll();

        NetworkManager.Instance.Connect(AppConst.SvrGameIp, AppConst.SvrGamePort);
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
