using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseConfig 
{
    public virtual void Init()
    { 
    
    }

    public virtual async UniTaskVoid InitUniTask()
    {
        await UniTask.Yield();
    }

    public async UniTask<T> LoadConfig<T>(Type type) where T : class
    {
        string path = "Assets/App/Config/" + type.Name.Replace("Config", "") + ".json";
        TextAsset ta = await ResManager.Instance.CommonLoadAssetAsync<TextAsset>(path);
        return LitJson.JsonMapper.ToObject<T>(ta.text);
    }
}
