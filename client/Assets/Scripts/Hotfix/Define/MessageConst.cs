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

    

    // 框架层100-200，逻辑层的 200-300

    //新手引导点击事件
    public const int Msg_GuideClickComplete = 201;
}
