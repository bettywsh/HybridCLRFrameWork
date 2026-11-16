using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPointReq
{
    public int code;
    public int channel;
}

public class PointManager : Singleton<PointManager>
{
    string url = $"{AppSettings.AppConfig.HttpUrl}api/Client/AddPoint";
    public void AddPoint(int code)
    {
        // var addPointReq = new AddPointReq();
        // addPointReq.code = code;
        // addPointReq.channel = AppSettings.AppConfig.ChannelId;
        // HttpManager.Instance.PostRequest(url, JsonConvert.SerializeObject(addPointReq), true, DataManager.Instance.GetData<UserInfoData>().PlayerBaseInfo.PlayerID.ToString()).Forget();
    }
}
