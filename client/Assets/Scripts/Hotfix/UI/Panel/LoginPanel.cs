using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Panel]
public class LoginPanel : PanelBase
{
    public override async UniTask OnOpen()
    { 
        await base.OnOpen();
    }

    [OnClick("btnLogin")]
    void OnLogin()
    { 
    
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
