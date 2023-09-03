using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MessageConst
{

    public const string Msg_Connected = "Msg_Connected"; //连接到服务器
    public const string Msg_LostConnect = "Msg_LostConnect"; //失去服务器
    public const string Msg_Disconnected = "Msg_Disconnected"; //断开服务器
       
    //public const string MsgNetData = "OnNetData";//收到网络数据的消息（还未分出Command Id的数据，在这里处理网络消息分发）
    //public const string MsgNetCmd = "OnNetCmd_";//收到网络数据的消息（已经分出Command Id的数据，在这里处理收到具体对应网络命令，比如：OnNetCmd_LC_Login,LC_Login是Login返回消息CommandId的定义）
    public const string Net_ = "Net_";// for c# use

    //游戏切换后台
    public const string Msg_ApplicationPause = "Msg_ApplicationPause";


    //unity退出事件
    public const string Msg_OnApplicationQuit = "Msg_OnApplicationQuit";


    //////////////////////////////////上面是框架事件其他写下面

    //新手引导点击事件
    public const string Msg_GuideClickComplete = "Msg_GuideClickComplete";
}
