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
    /// 强更版本
    /// </summary>
    public int AppVersion = 1;
    /// <summary>
    /// 资源版本
    /// </summary>
    public int ResVersion = 10001;

    /// <summary>
    /// 渠道id
    /// </summary>
    public int ChannelId = 8001;

    /// <summary>
    /// 热更下载地址
    /// </summary>
    public string SvrResIp = "http://192.168.11.18:8082/";

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
    /// 强更文件名
    /// </summary>
    public string DownloadApkName = "ff.apk";
}
