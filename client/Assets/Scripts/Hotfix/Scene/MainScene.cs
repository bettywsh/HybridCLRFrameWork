using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Scene]
public class MainScene : SceneBase
{
    public override void LoadScene()
    {
        UIManager.Instance.Open<MainPanel>();
    }

    public override void UnLoadScene()
    {
        
    }
}
