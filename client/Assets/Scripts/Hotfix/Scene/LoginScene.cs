using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScene : SceneBase
{
    public override void LoadScene()
    {
        UIManager.Instance.Open<LoginPanel>();
    }

    public override void UnLoadScene()
    {
        
    }
}
