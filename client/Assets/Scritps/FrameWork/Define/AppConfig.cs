using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

[CreateAssetMenu(fileName = "AppConfig", menuName = "AppConfig")]
public class AppConfig : ScriptableObject
{
    /// <summary>
    /// 运行模式
    /// </summary>
    ///
    public EPlayMode EPlayMode = EPlayMode.EditorSimulateMode;

    /// <summary>
    /// app版本
    /// </summary>
    ///
    public string AppVersion = "1.01";

    /// <summary>
    /// 资源版本
    /// </summary>
    ///
    public string ResVersion = "10001";

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

    /// <summary>
    /// ProtoBuff 包名
    /// </summary>
    public string ProtoBuffPackageName = "com.bochsler.protocol.";

    /// <summary>
    /// 启动场景名
    /// </summary>
    public string StartSceneName = "Start";
}
