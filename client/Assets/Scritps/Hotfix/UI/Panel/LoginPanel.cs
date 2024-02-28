using com.bochsler.protocol;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanel : BasePanel
{
    public override async UniTask OnOpen()
    {
        await base.OnOpen();
        //ObjectHelper.SetGrey(view.btn_Ok.transform, true);
    }

    [OnMessage("OnMsg_Connected")]
    public void OnMsg_Connected(object[] msgDatas)
    {
        Debug.LogError("连接成功");
        LoginRequest lr = new LoginRequest();
        lr.sceneType = SceneType.SceneTypeGALLERY;
        lr.Password = "";
        lr.Username = "6422bf99fad65bd0a84c10ab";
        lr.loginType = 0;
        NetworkManager.Instance.Send((int)CSMessageEnum.LoginRequest, lr);
    }

    [OnNet((int)SCMessageEnum.LoginResponse)]
    public void OnNet_LoginResponse(object[] msgDatas)
    {
        Debug.LogError("LoginResponse");
    }

    [OnClick("Ok")]
    public void OnClick_Ok()
    {
        //List<HorseConfigItem> list = ConfigManager.Instance.LoadConfig<HorseConfig>().GetAll();

        //NetworkManager.Instance.Connect(AppConst.SvrGameIp, AppConst.SvrGamePort);

        LoadSceneManager.Instance.LoadScene(EScene.Main.ToString(), false);
        this.Close();
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
