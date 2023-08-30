using System.Collections.Generic;
using UnityEngine;

public class ConfigManager : Singleton<ConfigManager>
{    
    public ChineseTextConfig chineseTextConfig = new ChineseTextConfig();


    public override void Init()
    {
        chineseTextConfig.Init(LoadConfig<List<ChineseTextConfigItem>>("ChineseText"));

    }

    public T LoadConfig<T>(string fileName) where T : class
    {
        //TextAsset ta = ResManager.Instance.LoadAsset("Common", "Config/" + fileName, typeof(TextAsset)) as TextAsset;
        //return LitJson.JsonMapper.ToObject<T>(ta.text);
        //return FirCommon.Utility.ProtoUtil.Deserialize<T>(fullPath);
        return default;
    }

}
