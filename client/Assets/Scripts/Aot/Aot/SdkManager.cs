using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SdkManager : AotMonoSingleton<SdkManager>
{
    #region 支付宝授权
    public static void AliAuth(string url)
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("AliAuth", url);
#endif

    }

    //支付宝授权成功
    public void AliAuthSucc(string authCode)
    {
        Debug.Log("authCode:" + authCode);
        //GameMgr.Instance.OnNativeMsgGet("AliAuthSucc", authCode);
    }
    //支付宝授权失败
    public void AliAuthFail(string authCode)
    {
        Debug.Log("authCode:" + authCode);
        //GameMgr.Instance.OnNativeMsgGet("AliAuthFail", authCode);
    }
    #endregion

    #region 支付宝App支付
    public static void AliPay(string orderinfo)
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("AliPay", orderinfo);
#endif

    }
    #endregion

    #region 支付宝网页支付
    public static void GoToApp(string appUrl)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("LoadInnerApp", appUrl);
#endif
    }
    #endregion

    #region 快手
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

    #region 百度

    public static void BaiDu(int key, string value = "")
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("BaiDu", key, value);
#endif

    }
    #endregion

    #region 抖音

    public static void DouYin(int key, string value = "")
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("DouYin", key, value);
#endif

    }
    #endregion


}
