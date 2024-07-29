using Proto.basepb;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using System.Drawing.Printing;

public class LoginPanel : PanelBase
{
    public override async UniTask OnOpen()
    {
        await base.OnOpen();
        NetworkManager.Instance.Create(NetworkProtocol.WebSocket).Create(NetworkProtocol.WebSocket, EServer.Login, AppSettings.AppConfig.SvrGameIp, AppSettings.AppConfig.SvrGamePort);
    }

    [OnNet((int)SCMessageEnum.LoginResponse)]
    public void OnNet_LoginResponse(byte[] msgDatas)
    {
        Debug.LogError("LoginResponse");
    }

    [OnClick("LoginWX")]
    void OnClickLoginWX()
    {
        //List<HorseConfigItem> list = ConfigManager.Instance.LoadConfig<HorseConfig>().GetAll();


        LoginRequest lr = new LoginRequest();
        lr.Account = "ºÆºÆ57";
        lr.Passward = "e10adc3949ba59abbe56e057f20f883e";
        lr.Deviceid = MD5Helper.MD5String("964d1213-21be-591e-eee1-642a8ff7e525");
        lr.Channel = "8000";
        lr.Imei = "964d1213-21be-591e-eee1-642a8ff7e525";
        //RequestPhoneCode lr = new();
        //lr.phoneNumber = "13636316992";
        NetworkManager.Instance.GetSession(NetworkProtocol.WebSocket).Send(EServer.Login, (long)CSMessageEnum.LoginRequest, lr);

        //LoadSceneManager.Instance.LoadScene(EScene.Main.ToString(), false);
        //this.Close();
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
