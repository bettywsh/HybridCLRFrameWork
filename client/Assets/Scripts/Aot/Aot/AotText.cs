using Cysharp.Threading.Tasks;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AotText : AotSingleton<AotText>
{
    string language;
    JsonData data;
    public override async UniTask Init()
    {
        string startLanguage = PlayerPrefs.GetString("language", "");
        if (startLanguage == "")
        {
            language = startLanguage = "Chinese";
            PlayerPrefs.SetString("language", language);
        }
        else
        {
            language = startLanguage;
        }
        TextAsset ta = await AotResManager.Instance.LoadAsset<TextAsset>("Assets/App/Config/Language.json");
        data = LitJson.JsonMapper.ToObject(ta.text);
    }


    public string GetText(string key)
    {
        return data[key].ToString();
    }

    public override void Dispose()
    {
        base.Dispose();
        language = null;
        data = null;
    }
}
