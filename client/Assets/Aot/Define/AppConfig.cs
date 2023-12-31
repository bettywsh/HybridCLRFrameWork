using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

[CreateAssetMenu(fileName = "AppConfig", menuName = "YooAsset/AppConfig")]
public class AppConfig : ScriptableObject
{
    /// <summary>
    /// 运行模式
    /// </summary>
    ///
    public EPlayMode EPlayMode = EPlayMode.EditorSimulateMode;

    /// <summary>
    /// 是否开启热更新
    /// </summary>
    public bool UpdateModel = false;

    /// <summary>
    /// 热更下载地址
    /// </summary>
    public string SvrResIp = "http://192.168.11.18:8082/";

    /// <summary>
    /// 游戏启动ip地址
    /// </summary>
    public string SvrGameIp = "shiku.grandlink.net";

    /// <summary>
    /// 游戏启动端口
    /// </summary>
    public int SvrGamePort = 8443;

    /// <summary>
    /// 打印Log模式
    /// </summary>
    public bool DebugLog = true;

    /// <summary>
    /// 游戏帧频
    /// </summary>
    public int GameFrameRate = 30;

    /// <summary>
    /// yooasset 包名
    /// </summary>
    public string PackageName = "DefaultPackage";
}
