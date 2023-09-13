using com.bochsler.protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanel : BasePanel
{
	public LoginPanelView view;

    public override void OnBindEvent()
    {
        view = transform.GetComponent<LoginPanelView>();
        base.OnBindEvent();
    }

    public override void OnOpen()
    {
        base.OnOpen();
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


        //NetworkManager.Instance.Connect(AppConst.SvrGameIp, AppConst.SvrGamePort);

        UIManager.Instance.Open<MainPanel>();
        UIManager.Instance.Close<LoginPanel>();
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
