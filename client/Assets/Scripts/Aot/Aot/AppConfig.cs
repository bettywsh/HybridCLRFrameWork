using Sirenix.OdinInspector;
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
    public string SvrResIp = "http://192.168.14.149:8082/";

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
    /// ǿ���ļ���
    /// </summary>
    public string DownloadApkName = "ff.apk";

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

    /// <summary>
    /// ����Ԫ����
    /// </summary>
    public List<string> AotDll = new List<string>() { "mscorlib.dll", "System.Core.dll", "System.dll" };

    /// <summary>
    /// �ȸ�dll����
    /// </summary>
    public List<string> HotfixDll = new List<string>() { "Hotfix.dll", "FrameWork.dll" };

    public static string[] Modes = new string[] { "�༭��", "���Է�", "��ʽ��"};
    [OnValueChanged("SetMode")]
    [ValueDropdown("Modes")]
    public string Mode = "�༭��";

    public void SetMode()
    {
        if (Mode == "�༭��")
        {
            EPlayMode = EPlayMode.EditorSimulateMode;
        }
        else if (Mode == "���Է�")
        {
            EPlayMode = EPlayMode.HostPlayMode;
        }
    }
}
