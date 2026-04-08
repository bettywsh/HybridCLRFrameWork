using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[Scene]
public class LoginScene : SceneBase
{
    public override async void LoadScene()
    {
        //销毁aot管理器
        AotDialogManager.Instance.Dispose();
        AotHttpManager.Instance.Dispose();
        AotResManager.Instance.Dispose();
        AotUIManager.Instance.Close(typeof(UpdatePanel));
        AotUIManager.Instance.Dispose();


        base.LoadScene();
    }

    public override void UnLoadScene()
    {
        
    }
}
