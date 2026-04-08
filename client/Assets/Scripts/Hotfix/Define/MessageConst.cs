using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class MessageConst
{
    // 框架层100-200，逻辑层的从200起
    //连接到服务器
    public const int Msg_Connected = 101;
    //断开服务器
    public const int Msg_Disconnected = 102;
    //游戏切换后台
    public const int Msg_ApplicationPause = 103;
    //unity退出事件
    public const int Msg_ApplicationQuit = 104;
    //场景切换进度事件
    public const int Msg_LoadingPanelProgress = 105;
    //场景切换进度事件
    public const int Msg_LoadingPanelComplete = 106;
    //心跳包
    public const int Msg_HeartBeat = 107;

    //网络错误
    public const int Msg_NetError = 109;
    
    //重连成功消息
    public const int Msg_ReConnectSucc = 111;
    //关闭心跳包
    public const int Msg_HeartBeatClose = 112;
    //
    public const int Msg_ReConnectPanelClose = 113;
    public const int Msg_BattleLoadingPanelComplete = 114;
    public const int Msg_BattleLoadingPanelClose= 115;

    // 框架层100-200，逻辑层的 200-300

    public const int Msg_LoginSuccessfull = 202;
    public const int Msg_BuyShopItemRes = 204;
    public const int Msg_AttackMonsterRes = 205;
    public const int Msg_AutoAttack = 206;
    public const int Msg_UnLockHeroCardRes = 207;
    public const int Msg_MoveCoin = 208;
    public const int Msg_DungeonTip = 209;
    public const int Msg_ChangereSpar = 210;
    public const int Msg_UpEquip = 211;
    public const int Msg_HongBaoRes = 212;
    public const int Msg_GameEndRes = 213;
    public const int Msg_SupplyRes = 214;
    public const int Msg_VipLevelUp = 215;
    public const int Msg_RMBBuyGoods = 216;
    public const int Msg_GetSevenDayGiftRes = 217;
    public const int Msg_GetFirstRewardHeroRes = 218;
    public const int Msg_MuzzleChange = 219;
    public const int Msg_GetDaySignRewardRes = 220;
    public const int Msg_ReadMailRes = 221;
    public const int Msg_DeleteMailRes = 222;
    public const int Msg_NewMail = 223;
    public const int Msg_GetRewardRes = 224;
    public const int Msg_ReadAllMailRes = 225;
    public const int Msg_DeleteAllMailRes = 226;
    public const int Msg_TruntableResult = 227;
    public const int Msg_GetDayTaskRewardRes = 228;
    public const int Msg_GetRechargeCardRewardRes = 229;
    public const int Msg_RandStoneRes = 230;
    public const int Msg_JumpHallTab = 231;
    public const int Msg_BattlePopUIClose = 232;
    public const int Msg_HighlyProfitablePanelClose = 233;
    public const int Msg_UIClose = 234;
    public const int Msg_BulletExpChange = 235;
    public const int Msg_JackpotTimer = 236;
    public const int Msg_BossTimer = 237;
    public const int Msg_NewGuide = 238;
    public const int Msg_NewGuideComplete = 239;
    public const int Msg_ItemRewardClose = 240;
    public const int Msg_MoveMoDaoShi = 241;
    public const int Msg_NewGuideReshModaoshi = 242;
    public const int Msg_FristMainPopUIClose = 243;
    public const int Msg_MainPopUIClose = 244;
    public const int Msg_MagicSelectClose = 245;
    public const int Msg_GetTaskRewardRes = 246;
    public const int Msg_GuideShop = 247;
    public const int Msg_BossBoxOpenEnd = 248;
    public const int Msg_MagicLevelUp = 249;
    public const int Msg_NoticeTitleClick = 250;
    public const int Msg_PlayerResetName = 251;
    public const int Msg_PlayerResetHead = 252;
    public const int Msg_PlayerResetHeadOk = 253;
    public const int Msg_RageClose = 254;
    public const int Msg_PopUICannel = 255;
    public const int Msg_ShowWorldBanner = 256;
    public const int Msg_ChallengeTicketsChange = 257;
    public const int Msg_GetTaskAllReward = 258;
    public const int Msg_ProtocolAgreement = 259;
    public const int Msg_CloseFreeActivity = 260;

    public const int Msg_AliPaySucc = 300;
    public const int Msg_AliPayFail = 301;
    public const int Msg_AliAuthSucc = 302;
    public const int Msg_AliAuthFail = 303;
    public const int Msg_ShopOpen = 304;
    public const int Msg_ShopClose = 305;
    public const int Msg_ShopError = 306;
    public const int Msg_GuideAllComplete = 307;
    public const int Msg_TapTapLoginSucc = 308;
    public const int Msg_TapTapLoginFail = 309;
    public const int Msg_TapTapLoginCancel = 310;
    public const int Msg_EquipGiftRefresh = 311;
    public const int Msg_LimitedRefresh = 312;
    public const int Msg_GoToShop = 313;
    public const int Msg_VipGiftRefresh = 314;
    public const int Msg_GetVipActivityInfoRes = 315;
    public const int Msg_GoToEquip = 316;
    public const int Msg_EquipGiftClose = 317;
    public const int Msg_LimitedClose = 318;
    public const int Msg_VipGiftClose = 319;
    public const int Msg_GoToLevelOne = 320;
    public const int Msg_GoToLevelTwo = 321;
    public const int Msg_GoToLevelThree = 322;
    public const int Msg_LongwangActivityInfo = 323;
    public const int Msg_KillBossRewardInfo = 324;
    public const int Msg_RechargeFresh = 325;
    public const int Msg_EndGoodsRewardRes = 326;
    public const int Msg_ContinuousGoodsInfoRes = 327;
    public const int Msg_UnlockContinuousGoodsRes = 328;
    public const int Msg_GetActivityHeroRewardRes = 329;
    public const int Msg_GoToStrengthen = 330;
    public const int GetActivityEggInfoRes = 331;
    public const int HitEggRes = 332;
    public const int GetSignActivityInfoRes = 333;
    public const int Msg_ActivityTimeChange = 334;
    public const int Msg_Get0YuangouInfoRefsh = 335;
    public const int Msg_GetBlindBoxInfoRes = 336;
    public const int Msg_ResetBlindBoxRes = 337;
    public const int Msg_OpenBlindBoxRes = 338;
    public const int Msg_GetCrusadeDragonActivityInfoRes = 339;
    public const int Msg_GetSuperGiftInfoRes = 340;
    public const int Msg_GetMoneyboxInfoRes = 341;
    public const int Msg_GoHallSupPanel = 342;
    public const int Msg_OpenGameModeSubPanel = 343;
    public const int Msg_OpenHallSubPanel = 344;
    public const int Msg_ReshMonthCardLock = 345;
    public const int Msg_JinHeClear = 346;

    public const int Msg_RedPointRefresh = 400;
    public const int Msg_HeroUnlockNotifiy = 401;
    public const int Msg_EquipUpLevelNotifiy = 402;

    public const int Msg_BattleChooseSkillNum = 500;
    public const int Msg_BossDie = 501;
    public const int Msg_BossReward = 502;
    public const int Msg_GetAllTaskRewardRes = 503;

    //战都
    public const int Msg_ChangeZone = 1001;
    public const int Msg_OnClickSkillSlot = 1002;
    public const int Msg_ChangeLevel = 1003;
}
