using Sirenix.OdinInspector;
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
    public string SvrResIp = "http://192.168.14.149:8082/";

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

    /// <summary>
    /// ProtoBuff 包名
    /// </summary>
    public string ProtoBuffPackageName = "com.bochsler.protocol.";

    /// <summary>
    /// 游戏启动ip地址
    /// </summary>
    public string SvrGameIp = "shiku.grandlink.net";

    /// <summary>
    /// 游戏启动端口
    /// </summary>
    public int SvrGamePort = 8443;

    /// <summary>
    /// 补充元数据
    /// </summary>
    public List<string> AotDll = new List<string>() { "mscorlib.dll", "System.Core.dll", "System.dll" };

    /// <summary>
    /// 热更dll数据
    /// </summary>
    public List<string> HotfixDll = new List<string>() { "Hotfix.dll", "FrameWork.dll" };

    public static string[] Modes = new string[] { "编辑器", "测试服", "正式服"};
    [OnValueChanged("SetMode")]
    [ValueDropdown("Modes")]
    public string Mode = "编辑器";

    public void SetMode()
    {
        if (Mode == "编辑器")
        {
            EPlayMode = EPlayMode.EditorSimulateMode;
        }
        else if (Mode == "测试服")
        {
            EPlayMode = EPlayMode.HostPlayMode;
        }
    }
}
