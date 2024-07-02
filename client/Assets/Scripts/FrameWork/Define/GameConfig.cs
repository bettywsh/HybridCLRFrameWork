using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    /// <summary>
    /// ProtoBuff ����
    /// </summary>
    public string ProtoBuffPackageName = "com.bochsler.protocol.";

    /// <summary>
    /// ��Ϸ����ip��ַ
    /// </summary>
    public string SvrGameIp = "shiku.grandlink.net";

    /// <summary>
    /// ��Ϸ�����˿�
    /// </summary>
    public int SvrGamePort = 8443;
}
