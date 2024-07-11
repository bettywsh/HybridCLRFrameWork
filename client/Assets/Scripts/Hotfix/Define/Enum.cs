public enum EUILayer : int
{
    Normal = 0,
    
    Text = 110, //飘字
    Dialog = 111, //提示弹框
    Guide = 122, //新手引导
    Loading = 133, //切换场景
    DialogSystem = 151, //系统弹框
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
