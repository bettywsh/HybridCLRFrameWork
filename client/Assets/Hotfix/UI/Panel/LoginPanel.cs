using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class LoginPanel : BasePanel
{
    public void OnOpen()
    {
        DialogManager.Instance.ShowDialogOne("����", "��汾����������www.baidu.com", () => {
            Application.Quit();
        });
    }


    public void OnClose()
    {
        
    }

}
