using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

[CreateAssetMenu(fileName = "AppConfig", menuName = "AppConfig")]
public class AppConfig : ScriptableObject
{
    /// <summary>
    /// ����ģʽ
    /// </summary>
    ///
    public EPlayMode EPlayMode = EPlayMode.EditorSimulateMode;

    /// <summary>
    /// ǿ���汾
    /// </summary>
    public int AppVersion = 1;
    /// <summary>
    /// ��Դ�汾
    /// </summary>
    public int ResVersion = 10001;

    /// <summary>
    /// ����id
    /// </summary>
    public int ChannelId = 8001;

    /// <summary>
    /// �ȸ����ص�ַ
    /// </summary>
    public string SvrResIp = "http://192.168.11.18:8082/";

    /// <summary>
    /// ��Ϸ����ip��ַ
    /// </summary>
    public string SvrGameIp = "shiku.grandlink.net";

    /// <summary>
    /// ��Ϸ�����˿�
    /// </summary>
    public int SvrGamePort = 8443;

    /// <summary>
    /// ��ӡLogģʽ
    /// </summary>
    public bool DebugLog = true;

    /// <summary>
    /// ��Ϸ֡Ƶ
    /// </summary>
    public int GameFrameRate = 30;

    /// <summary>
    /// yooasset ����
    /// </summary>
    public string PackageName = "DefaultPackage";

    /// <summary>
    /// ProtoBuff ����
    /// </summary>
    public string ProtoBuffPackageName = "com.bochsler.protocol.";

    /// <summary>
    /// ǿ���ļ���
    /// </summary>
    public string DownloadApkName = "ff.apk";
}
