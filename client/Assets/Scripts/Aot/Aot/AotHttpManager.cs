using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

public class AotHttpManager : AotSingleton<AotHttpManager>
{
    /// <summary>
    /// 时间戳计时开始时间
    /// </summary>
    private static DateTime timeStampStartTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// DateTime转换为10位时间戳（单位：秒）
    /// </summary>
    /// <param name="dateTime"> DateTime</param>
    /// <returns>10位时间戳（单位：秒）</returns>
    public static long DateTimeToTimeStamp()
    {
        DateTime DateStart = new DateTime(1970, 1, 1, 8, 0, 0);
        return Convert.ToInt32((DateTime.UtcNow - DateStart).TotalSeconds);
    }

    //判断网络的状态
    public bool NetState()
    {
        //当网络不可用时                
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return false;
        }

        return true;
    }

    #region Get请求

    public async UniTask<string> GetRequest(string url, string token)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "GET"))
        {
            webRequest.timeout = 30;
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded;charset=gb2312");
            webRequest.SetRequestHeader("Time-Tamp", DateTimeToTimeStamp().ToString());
            if (token != null)
                webRequest.SetRequestHeader("Token", token);
            webRequest.SetRequestHeader("Time-Zone", System.TimeZone.CurrentTimeZone.GetUtcOffset(System.DateTime.Now).Hours.ToString());
            try
            {
                await webRequest.SendWebRequest();
            }
            catch
            {
                return "";
            }
            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(webRequest.error + "\n" + webRequest.downloadHandler.text + "   " + url);
                return "";
            }
            else
            {
                string data = webRequest.downloadHandler.text;
                webRequest.Dispose();
                return data;
            }
        }
    }
    #endregion

    #region Post请求

    public async UniTask<string> PostRequest(string url, string jsonString, string token)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonString));
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded;charset=utf8");
            webRequest.SetRequestHeader("Time-Tamp", DateTimeToTimeStamp().ToString());
            webRequest.SetRequestHeader("Token", token);
            webRequest.SetRequestHeader("Time-Zone", System.TimeZone.CurrentTimeZone.GetUtcOffset(System.DateTime.Now).Hours.ToString());

            try
            {
                await webRequest.SendWebRequest().ToUniTask();
            }
            catch
            {
                return "";
            }

            if (webRequest.result == UnityWebRequest.Result.ProtocolError || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(webRequest.error + "\n" + webRequest.downloadHandler.text);
                return "";
            }
            else
            {
                string data = webRequest.downloadHandler.text;
                webRequest.Dispose();
                return data;
            }
        }
    }
    #endregion
}
