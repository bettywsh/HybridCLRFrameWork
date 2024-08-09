using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SdkManager : AotMonoSingleton<SdkManager>
{
    #region ֧������Ȩ
    public static void AliAuth(string url)
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("AliAuth", url);
#endif

    }

    //֧������Ȩ�ɹ�
    public void AliAuthSucc(string authCode)
    {
        Debug.Log("authCode:" + authCode);
        //GameMgr.Instance.OnNativeMsgGet("AliAuthSucc", authCode);
    }
    //֧������Ȩʧ��
    public void AliAuthFail(string authCode)
    {
        Debug.Log("authCode:" + authCode);
        //GameMgr.Instance.OnNativeMsgGet("AliAuthFail", authCode);
    }
    #endregion

    #region ֧����App֧��
    public static void AliPay(string orderinfo)
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("AliPay", orderinfo);
#endif

    }
    #endregion

    #region ֧������ҳ֧��
    public static void GoToApp(string appUrl)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("LoadInnerApp", appUrl);
#endif
    }
    #endregion

    #region ����
    public static void KuaiShou(int key, string value = "")
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("KuaiShou", key, value);
#endif

    }

    public static void KuaiShouGuanGao(int key)
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("KuaiShouGuanGao", key);
#endif

    }
    #endregion

    #region �ٶ�

    public static void BaiDu(int key, string value = "")
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("BaiDu", key, value);
#endif

    }
    #endregion

    #region ����

    public static void DouYin(int key, string value = "")
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("DouYin", key, value);
#endif

    }
    #endregion


}
