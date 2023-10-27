using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

[CreateAssetMenu(fileName = "AppConfig", menuName = "YooAsset/AppConfig")]
public class AppConfig : ScriptableObject
{
    /// <summary>
    /// ����ģʽ
    /// </summary>
    ///
    public EPlayMode EPlayMode = EPlayMode.EditorSimulateMode;

    /// <summary>
    /// �Ƿ����ȸ���
    /// </summary>
    public bool UpdateModel = false;

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
}
