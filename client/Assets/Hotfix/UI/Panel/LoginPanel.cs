using com.bochsler.protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanel : BasePanel
{

    public override void OnBindEvent()
    {        
        base.OnBindEvent();
    }

    public override void OnOpen()
    {
        base.OnOpen();
        //ObjectHelper.SetGrey(view.btn_Ok.transform, true);
    }

    void OnMsg_Connected(object[] msgDatas)
    {
        Debug.LogError("连接成功");
        LoginRequest lr = new LoginRequest();
        lr.sceneType = SceneType.SceneTypeGALLERY;
        lr.Password = "";
        lr.Username = "6422bf99fad65bd0a84c10ab";
        lr.loginType = 0;
        NetworkManager.Instance.Send(CSMessageEnum.LoginRequest, lr);
    }

    void OnNet_LoginResponse(object[] msgDatas)
    {
        Debug.LogError("LoginResponse");
    }

    void OnClick_Ok()
    {
        //List<HorseConfigItem> list = ConfigManager.Instance.LoadConfig<HorseConfig>().GetAll();

        //NetworkManager.Instance.Connect(AppConst.SvrGameIp, AppConst.SvrGamePort);

        LoadSceneManager.Instance.LoadScene(EScene.Main, false);
        UIManager.Instance.Close<LoginPanel>();
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
