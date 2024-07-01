using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MessageConst
{
    //连接到服务器
    public const string Msg_Connected = "Msg_Connected";
    //断开服务器
    public const string Msg_Disconnected = "Msg_Disconnected";
    //游戏切换后台
    public const string Msg_ApplicationPause = "Msg_ApplicationPause";
    //unity退出事件
    public const string Msg_ApplicationQuit = "Msg_ApplicationQuit";
    //场景切换进度事件
    public const string Msg_LoadingPanelProgress = "Msg_LoadingPanelProgress";
    //场景切换进度事件
    public const string Msg_LoadingPanelComplete= "Msg_LoadingPanelComplete";


    //////////////////////////////////上面是框架事件其他写下面
    //新手引导点击事件
    public const string Msg_GuideClickComplete = "Msg_GuideClickComplete";
}
