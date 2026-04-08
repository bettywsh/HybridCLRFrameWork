public enum EUILayer : int
{
    Normal = 0,

    Reward = 207, //获得奖励
    Tips = 208, //获得奖励提示
    HeroTips = 209, //获得奖励提示
    WorldBanner = 210, //跑马灯
    Text = 220, //飘字
    Dialog = 221, //提示弹框    
    Guide = 222, //新手引导
    NetLoding = 241,//网络菊花
    Loading = 242, //切换场景
    ReConnectLoadingPanel = 243,//重连
    DialogSystem = 251, //系统弹框
}


public enum EScene : int
{ 
    Login = 1,
    Main,
    Battle,
}

public enum EAttribute : int
{
    Scene = 1,
    Panel,
    SubPanel,
    Cell
}

public enum EntranceType : int
{
    None,
    Hall,
    Shop,
    Heroes,
    Exchange,
    Equipment
}

public enum BuyItemType: int
{
    None,
    Diamond,
    Hero,
    HeroStone,
    Modaoshi,
    ExchangeMoBi
}
