using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Scene]
public class BattleScene : SceneBase
{

    public override async void LoadScene()
    {
        base.LoadScene();
        EventManager.Instance.MessageNotify(MessageConst.Msg_BattleLoadingPanelComplete);
    }

    public override async void UnLoadScene()
    {

    }
}
