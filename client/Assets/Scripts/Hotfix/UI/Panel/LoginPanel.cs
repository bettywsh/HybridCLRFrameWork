using com.bochsler.protocol;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class LoginPanel : PanelBase
{
    public override async UniTask OnOpen()
    {
        await base.OnOpen();       
    }

    [OnMessage(MessageConst.Msg_Connected)]
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
    void OnNet_LoginResponse(object[] msgDatas)
    {
        Debug.LogError("LoginResponse");
    }

    [OnClick("btnOk")]
    void OnClick_Ok()
    {
        //List<HorseConfigItem> list = ConfigManager.Instance.LoadConfig<HorseConfig>().GetAll();

        //NetworkManager.Instance.Connect(AppConst.SvrGameIp, AppConst.SvrGamePort);

        LoadSceneManager.Instance.LoadScene(EScene.Main.ToString(), false);
        this.Close();
        cancellationTokenSource.Cancel();
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
