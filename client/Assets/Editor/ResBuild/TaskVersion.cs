using System.IO;
using UnityEditor;
using LitJson;
using UnityEngine;

public class TaskVersion : ITask
{
    public void Run(PackSetting packSetting)
    {
        string version = File.ReadAllText(ResPath.AppFullPath + "/" + ResConst.VerFile);

        JsonData jd = JsonMapper.ToObject(version);
        if (packSetting.IsHotfix)
        {
            jd["ResVersion"] = int.Parse(jd["ResVersion"].ToString()) + 1;
        }
        else
        {
            jd["ResVersion"] = 1;
            jd["MainVersion"] = (int)jd["MainVersion"] + 1;
            string[] gameVers = jd["GameVersion"].ToString().Split(".");
            jd["GameVersion"] = $"{gameVers[0]}.{gameVers[1]}.{jd["MainVersion"].ToString()}";
        }

        File.WriteAllText($"{ResPath.AppFullPath}/{ResConst.VerFile}", JsonMapper.ToJson(jd), new System.Text.UTF8Encoding(false));
        if (packSetting.IsHotfix)
        {
            File.WriteAllText($"{ResPack.BuildHotfixPath}/{ResConst.RootFolderName.ToLower()}/{ResConst.VerFile}", JsonMapper.ToJson(jd), new System.Text.UTF8Encoding(false));
        }
        else
        {
            PlayerSettings.bundleVersion = jd["GameVersion"].ToString();
            File.WriteAllText($"{ResPack.BuildCreatePath}/{ResConst.RootFolderName.ToLower()}/{ResConst.VerFile}", JsonMapper.ToJson(jd), new System.Text.UTF8Encoding(false));
        }
        AssetDatabase.Refresh();
    }
}
