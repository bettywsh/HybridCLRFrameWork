using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SDKManager : MonoSingleton<SDKManager>
{
    public string AndroidOAID;
    public string ImeiID;
    public string Imei2ID;
    public string MeID;
    #region 支付宝授权
    public void AliAuth(string url)
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("AliAuth", url);
#endif

    }

    //支付宝授权成功
    public void AliAuthSucc(string authCode)
    {
        Debug.Log("AliAuthSucc:" + authCode);
        EventManager.Instance.MessageNotify(MessageConst.Msg_AliAuthSucc, authCode);
    }
    //支付宝授权失败
    public void AliAuthFail(string authCode)
    {
        Debug.Log("AliAuthFail:" + authCode);
        EventManager.Instance.MessageNotify(MessageConst.Msg_AliAuthFail, authCode);
    }
    #endregion

    #region 支付宝App支付
    public void AliPay(string orderinfo)
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("AliPay", orderinfo);
#endif

    }

    //支付宝授权成功
    public void AliPaySucc(string authCode)
    {
        Debug.Log("AliPaySucc:" + authCode);
        EventManager.Instance.MessageNotify(MessageConst.Msg_AliPaySucc, authCode);
    }
    //支付宝授权失败
    public void AliPayFail(string authCode)
    {
        Debug.Log("AliPayFail:" + authCode);
        EventManager.Instance.MessageNotify(MessageConst.Msg_AliPayFail, authCode);
    }
    #endregion

    #region 商场

    public void InitShop(string dev, string appId, string token)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("InitShop", dev, appId, token);
#endif
    }

    public void ShowShop()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("ShowShop");
#endif
    }

    public void HideShop()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("HideShop");
#endif
    }

    //1打开2关闭3初始化失败
    public void ShowShopOpen(string status)
    {
        Debug.Log("ShowShopOpen:" + status);
        EventManager.Instance.MessageNotify(MessageConst.Msg_ShopOpen);
    }
    public void ShowShopClose(string status)
    {
        Debug.Log("ShowShopClose:" + status);
        EventManager.Instance.MessageNotify(MessageConst.Msg_ShopClose);
    }
    public void ShowShopError(string status)
    {
        Debug.Log("ShowShopError:" + status);
        EventManager.Instance.MessageNotify(MessageConst.Msg_ShopError);
    }
    #endregion

    #region TapTap

    public void TapTapInit()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("TapTapInit");
#endif
    }

    public void TapTapLogin()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("TapTapLogin");
#endif
    }

    public void TapTapLoginSucc(string openid)
    {
        Debug.Log("TapTapLoginSucc:" + openid);
        EventManager.Instance.MessageNotify(MessageConst.Msg_TapTapLoginSucc, openid);
    }

    public void TapTapLoginFail(string openid)
    {
        Debug.Log("TapTapLoginFail:" + openid);
        EventManager.Instance.MessageNotify(MessageConst.Msg_TapTapLoginFail);
    }

    public void TapTapLoginCancel(string openid)
    {
        Debug.Log("TapTapLoginCancel:" + openid);
        EventManager.Instance.MessageNotify(MessageConst.Msg_TapTapLoginCancel);
    }

    public void TapTapLoginOut()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("TapTapLoginOut");
#endif
    }

    #endregion

    #region 快手
    public void KuaiShou(int key, string value)
    {
        if (AppSettings.AppConfig.SdkDev)
            return;
#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidClass.UnityJavaObject.Call("KuaiShou", key, value);
#endif
    }
    #endregion

    #region 快手渠道
    public void KuaiShouChannel()
    {
        if (AppSettings.AppConfig.SdkDev)
            return;
        if (AppSettings.AppConfig.ChannelId == 3)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            AndroidClass.UnityJavaObject.Call("GetKuaiShouChannel");
#endif
        }
    }

    public void GetKuaiShouChannel(string channelid)
    {       
        Debug.Log("GetShouChannelID:" + channelid);
        if (channelid != "")
            AppSettings.AppConfig.ChannelId = int.Parse(channelid);
    }
    #endregion

    #region 安卓oaid
    public void InitAndroidOAID()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            AndroidClass.UnityJavaObject.Call("InitAndroidOAID");
        }
        catch
        {

        }
    #endif
    }

    public void GetAndroidOAID(string oaid)
    {
        Debug.Log("GetAndroidOAID:" + oaid);
        AndroidOAID = oaid;
    }

    public void InitImei()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("InitImei");
#endif
    }

    public void GetImeiID(string imeiid)
    {
        Debug.Log("GetAndroidOAID:" + imeiid);
        ImeiID = imeiid;
    }

    public void GetImei2ID(string imeiid)
    {
        Debug.Log("GetAndroidOAID:" + imeiid);
        Imei2ID = imeiid;
    }

    public void GetMeID(string meid)
    {
        Debug.Log("GetAndroidOAID:" + meid);
        MeID = meid;
    }
#endregion

    #region Bugly
    public void InitBugly()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("InitBugly");
#endif
    }

    public void TestJavaCrash()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidClass.UnityJavaObject.Call("TestJavaCrash");
#endif
    }
    #endregion
}
