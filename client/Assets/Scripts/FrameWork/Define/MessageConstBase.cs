public class MessageConstBase
{
    // 框架层100-200
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
    public const int Msg_BattleLoadingPanelClose = 115;
}
