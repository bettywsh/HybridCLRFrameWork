using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class BaseConfig 
{
    public virtual async UniTask Init()
    { 
        await UniTask.Yield();
    }

    public async UniTask<T> LoadConfig<T>(Type type) where T : class
    {
        string path = "Assets/App/Config/" + type.Name.Replace("Config", "") + ".json";
        TextAsset ta = await ResManager.Instance.CommonLoadAssetAsync<TextAsset>(path);
        return JsonConvert.DeserializeObject<T>(ta.text);
    }
}
