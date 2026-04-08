using cfg;
using Msg;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Data]
public class WorldBannerData : DataBase
{
    private Queue<string> MsgQueue = new Queue<string>();
    public override void Init()
    {
        base.Init();
        EventHelper.RegisterNetEvent(this);
        EventHelper.RegisterTimerEvent(this);
        TimerManager.Instance.OnceTimer(TimerConst.WorldBannerTimer, 1);
    }

    [OnNet((int)MsgID.MSGShowPaomadengMsg)]
    void OnNetNewServerDay(byte[] msgDatas)
    {
        ShowPaomadengMsg showPaomadengMsg = ProtobufHelper.Deserialize<ShowPaomadengMsg>(msgDatas);
        string content = "";
        if (showPaomadengMsg.TargetType == ShowPaomadengMsg.TriggerTargetType.TTTActMonsterDropSpar)
        {
            cfg.Session session = ConfigManager.Instance.Tables.SessionConfig.DataList.Find((x) => { return x.Type == showPaomadengMsg.TriggerPlayer.AtScene; });
            cfg.AI ai = ConfigManager.Instance.Tables.AIConfig.DataList.Find((x) => { return x.Id == showPaomadengMsg.TargetID; });
            content = $"尊贵的<sprite=10>{TextMeshProHelper.TextToSprite(showPaomadengMsg.TriggerPlayer.PlayerVip.ToString())}玩家<color=#fff600>{showPaomadengMsg.TriggerPlayer.PlayerName}</color>在<color=#19ff20>{session.Name}</color>打败了<color=#ffa453>{ai.Name}</color>获得了<color=#fff600>晶币x{showPaomadengMsg.Number}</color>，太帅了！";
        }
        else if (showPaomadengMsg.TargetType == ShowPaomadengMsg.TriggerTargetType.TTTActivityGetMoney)
        {
            content = $"尊贵的<sprite=10>{TextMeshProHelper.TextToSprite(showPaomadengMsg.TriggerPlayer.PlayerVip.ToString())}玩家<color=#fff600>{showPaomadengMsg.TriggerPlayer.PlayerName}</color>在<color=#19ff20>满月拾礼</color>活动中获得了<color=#fff600>晶币x{showPaomadengMsg.Number}</color>，太帅了！";
        }
        MsgQueue.Enqueue(content);
    }

    [OnTimer(TimerConst.WorldBannerTimer)]
    public async void AddMsg()
    {
        TimerManager.Instance.OnceTimer(TimerConst.WorldBannerTimer, 1);

        if (MsgQueue.Count <= 0)
            return;
        EventManager.Instance.MessageNotify(MessageConst.Msg_ShowWorldBanner);
    }

    public string GetMsg()
    {
        return MsgQueue.Dequeue();
    }


    public override void Reset()
    {
        base.Reset();
    }
}
