using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : Singleton<TextManager>
{
    string language;
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
    }

    public void ChangeLanguage(string languageName)
    {
        language = languageName;
        PlayerPrefs.SetString("language", languageName);
    }

    public string GetText(string key)
    {
        LanguageConfigItem languageConfigItem = ConfigManager.Instance.LoadConfig<LanguageConfig>().GetById(key);
        string str = "Not Found" + key;
        try
        {
            str = typeof(LanguageConfigItem).GetField(language).GetValue(languageConfigItem) as string;
        }
        catch {

        }
        return str;
    }
}
