using System.Collections.Generic;

/// <summary>
/// 不弹 NetLoading 的消息号（发送不 Show，这些号的回包也不 Hide）。
/// 数值与 Hotfix Msg.MsgID 对应，FrameWork 不能引用 Hotfix，故用字面量。
/// </summary>
public static class NoNetLoadingMsgIds
{
    public static readonly HashSet<long> Ids = new HashSet<long>
    {
        1,   // MSGHeartBeat
        6,   // MSGEnterGameRes
        7,   // MSGAttackMonsterReq
        8,   // MSGAttackMonsterRes
        15,  // MSGUpgradeEquipReq
        16,  // MSGUpgradeEquipRes
        22,  // MSGEndelessBoosDie
        23,  // MSGEndelessBoosReward
        27,  // MSGChangeGuide
        78,  // MSGGetMoneyToAliEnd
        81,  // MSGModaoshiPoolNotice
        115, // MSGGetLongwangActivityInfo
        116, // MSGLongwangActivityInfo
        120, // MSGGetRechargeActivityInfo
        121, // MSGGetRechargeActivityRewardRes
        126, // MSGGetPlayerRankInfo
        127, // MSGGetPlayerRankInfoRes
        138, // MSGHitEggReq
        139, // MSGHitEggRes
        140, // MSGGetActivityEggInfo
        141, // MSGGetActivityEggInfoRes
    };

    public static bool Contains(long msgId)
    {
        return Ids.Contains(msgId);
    }
}
