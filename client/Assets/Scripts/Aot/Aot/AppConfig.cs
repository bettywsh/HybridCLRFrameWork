using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    /// 渠道
    /// </summary>
    public int ChannelId = 1;

    /// <summary>
    /// 渠道
    /// </summary>
    public int ChannelVer = 1;

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
    /// 是否走APP支付
    /// </summary>
    public bool hasAppPay = false;

    /// <summary>
    /// HttpApi
    /// </summary>
    public bool SdkDev = true;

    /// <summary>
    /// HttpApi
    /// </summary>
    public string HttpUrl = "";

    /// <summary>
    /// 补充元数据
    /// </summary>
    public List<string> AotDll = new List<string>() { "mscorlib.dll", "System.Core.dll", "System.dll" };

    /// <summary>
    /// 热更dll数据
    /// </summary>
    public List<string> HotfixDll = new List<string>() { "FrameWork.dll", "Hotfix.dll" };

#if UNITY_EDITOR
    public static string[] Modes = new string[] { "测试服", "直播", "TapTap", "快手服"};
    [OnValueChanged("SetMode")]
    [ValueDropdown("Modes")]
    public string Mode = "测试服";

    public void SetMode()
    {
        if (Mode == "测试服")
        {
            EPlayMode = EPlayMode.EditorSimulateMode;
            DebugLog = true;
            SvrResIp = "https://oss.dongfanglanyu.com/zhanche/develop/";
            SvrGameIp = "121.196.227.83";
            HttpUrl = "http://47.101.186.85:83/";
            hasAppPay = false;
            SdkDev = true;
            ChannelId = 1;
            ChannelVer = 1;
            DownloadApkName = "ff.apk";
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.mtgame.xxm");
            PlayerSettings.productName = "新项目";

            Texture2D[] textures = new Texture2D[1];
            textures[0] = AssetDatabase.LoadAssetAtPath<Texture2D>($"Assets/Res/Icon/{ChannelId}.png");
            PlayerSettings.SetIcons(UnityEditor.Build.NamedBuildTarget.Unknown, textures, IconKind.Application);
        }
        else if (Mode == "直播")
        {
            EPlayMode = EPlayMode.OfflinePlayMode;
            DebugLog = false;
            SvrResIp = "https://oss.dongfanglanyu.com/client/hsmxwtest/";
            SvrGameIp = "47.102.112.62";
            HttpUrl = "http://47.101.186.85/";
            hasAppPay = false;
            SdkDev = true;
            ChannelId = 1;
            ChannelVer = 1;
            DownloadApkName = "ff.apk";
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.mtgame.com");
            PlayerSettings.productName = "斗龙战士3-天降小怪兽";

            Texture2D[] textures = new Texture2D[1];
            textures[0] = AssetDatabase.LoadAssetAtPath<Texture2D>($"Assets/Res/Icon/{ChannelId}.png");
            PlayerSettings.SetIcons(UnityEditor.Build.NamedBuildTarget.Unknown, textures, IconKind.Application);
        }
        else if (Mode == "TapTap")
        {
            EPlayMode = EPlayMode.OfflinePlayMode;
            DebugLog = false;
            SvrResIp = "https://oss.dongfanglanyu.com/client/hsmxwtest/";
            SvrGameIp = "dlzs3tjxgs.gametestserver.shanghailanyu.com";
            HttpUrl = "http://47.101.186.85/";
            hasAppPay = false;
            SdkDev = true;
            ChannelId = 2;
            ChannelVer = 2;
            DownloadApkName = "ff.apk";
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.mtgame.com");
            PlayerSettings.productName = "斗龙战士3-天降小怪兽";

            Texture2D[] textures = new Texture2D[1];
            textures[0] = AssetDatabase.LoadAssetAtPath<Texture2D>($"Assets/Res/Icon/{ChannelId}.png");
            PlayerSettings.SetIcons(UnityEditor.Build.NamedBuildTarget.Unknown, textures, IconKind.Application);
        }
        else if (Mode == "快手服")
        {
            EPlayMode = EPlayMode.HostPlayMode;
            DebugLog = false;
            SvrResIp = "https://oss.dongfanglanyu.com/client/hsmxw/";
            SvrGameIp = "dlzs3tjxgs.gameolineserver.shanghailanyu.com";
            HttpUrl = "https://manage.shanghailanyu.com/";
            hasAppPay = true;
            SdkDev = false;
            ChannelId = 3;
            ChannelVer = 1;
            DownloadApkName = "ff_3.apk";
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.mtgame.com");
            PlayerSettings.productName = "斗龙战士3-天降小怪兽";
            Texture2D[] textures = new Texture2D[1];
            textures[0] = AssetDatabase.LoadAssetAtPath<Texture2D>($"Assets/Res/Icon/{ChannelId}.png");
            PlayerSettings.SetIcons(UnityEditor.Build.NamedBuildTarget.Unknown, textures, IconKind.Application);
        }
    }
#endif
}
