using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : BaseScene
{
    public override void LoadScene()
    {
        UIManager.Instance.Open<MainPanel>();
    }

    public override void UnLoadScene()
    {
        
    }
}
