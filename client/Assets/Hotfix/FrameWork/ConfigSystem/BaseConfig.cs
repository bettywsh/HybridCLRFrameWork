using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseConfig 
{
    public virtual void Init()
    { 
    
    }

    public T LoadConfig<T>(Type type) where T : class
    {
        string path = "Config/" + type.Name.Replace("Config", "") + ".json";
        TextAsset ta = ResManager.Instance.LoadAsset("Common", path, typeof(TextAsset)) as TextAsset;
        return LitJson.JsonMapper.ToObject<T>(ta.text);
    }
}
