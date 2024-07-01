using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SDKManager : MonoBehaviour
{

    protected static SDKManager instance = null;
    public static SDKManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = GameObject.Find("SDKManager");
                if (go == null)
                {
                    go = new GameObject("SDKManager");
                    DontDestroyOnLoad(go);
                }
                instance = go.GetComponent<SDKManager>();
                if (instance == null)
                {
                    instance = go.AddComponent<SDKManager>();
                }
            }
            return instance;
        }
    }

    public void Init()
    {

    }

    //SDK 初始化
    public void InitSDK()
    {

    }

    //SDK 登录
    public void Login()
    {
        
    }
    //SDK 登录回调
    private void LoginCallBack(string openid)
    {
        //TODO 调用lua 层调用游戏登陆逻辑
    }

    //SDK 退出
    public void Logout()
    {

    }

    //SDK 退出回调
    private void LogoutCallBack()
    {
        //TODO 调用lua 层调用游戏逻辑
    }

    // SDK 支付
    public void Pay( string uid,string productID,string money,string des)
    {

    }
    // SDK 支付回调
    private void PayCallBack()
    {
        //TODO 调用lua 层调用游戏逻辑
    }

    //数据埋点
    public void BuryingPoint(string key,string value)
    {

    }

}
