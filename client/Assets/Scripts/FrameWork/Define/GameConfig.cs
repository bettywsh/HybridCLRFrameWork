using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "GameConfig")]
public class GameConfig : ScriptableObject
{
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
}
