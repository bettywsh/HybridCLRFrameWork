using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LoginPanel : BasePanel
{
    public void OnOpen()
    {
        DialogManager.Instance.ShowDialogOne("警告", "进入游戏www.baidu.com", () => {
            Application.Quit();
        });
    }


    public void OnClose()
    {
        
    }

}
