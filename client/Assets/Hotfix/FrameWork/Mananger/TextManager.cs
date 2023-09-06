using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : Singleton<TextManager>
{
    string language;
    LanguageConfigItem languageConfigItem;
    Type languageConfig;
    public override void Init()
    {
        base.Init();
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
        LanguageConfigItem languageConfigItem = ConfigManager.Instance.LoadConfig<LanguageConfig>().GetById(language);
        languageConfig = AotHybridCLR.Instance._hotUpdateAss.GetType(languageConfigItem.LanguageTable);
    }

    public string GetText(string languageName)
    {
        //ConfigManager.Instance.LoadConfig(languageConfig).GetById(languageName);
        return "";
    }

    //    function TextMgr:GetText(key)
    //    self.textDefine = require("Config/" .. self.LanguageTable[self.language].LanguageTable)
    //    if self.textDefine[key] == nil then
    //        return "Not Found" .. key
    //    else
    //        return self.textDefine[key].Value
    //    end
    //end

}
