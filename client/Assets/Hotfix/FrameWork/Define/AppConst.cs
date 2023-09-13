using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppConst
{

    /// <summary>
    /// 游戏启动ip地址
    /// </summary>
    public const string SvrGameIp = "shiku.grandlink.net";

    /// <summary>
    /// 游戏启动端口
    /// </summary>
    public const int SvrGamePort = 8443;

    /// <summary>
    /// 打印Log模式
    /// </summary>
    public const bool DebugLog = true;

    /// <summary>
    /// 游戏帧频
    /// </summary>
    public const int GameFrameRate = 30;

    /// <summary>
    /// 游戏ID
    /// </summary>
    public const int GameID = 10001;

    /// <summary>
    /// 渠道ID
    /// </summary>
    public const int ChannelID = 10001;


    /// <summary>
    /// 网络类型
    /// </summary>
    public static NetworkProtocol NetProtocol = NetworkProtocol.TCP;

}
