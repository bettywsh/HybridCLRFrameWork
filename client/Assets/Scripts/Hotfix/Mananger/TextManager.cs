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
        await base.Init();
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
        cfg.Language languageItem = ConfigManager.Instance.Tables.LanguageConfig.Get(key);
        if (languageItem != null)
        {
            return languageItem.Chinese;
        }
        else
        { 
            return "Not Found" + key;
        }
    }
}
