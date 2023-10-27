using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Threading;

public class AotUpdate : Singleton<AotUpdate>
{
    private string sJsonStr = "";
    List<DownLoadFile> needUpdate = new List<DownLoadFile>();
    private string writeClientFiles = "";
    string persistentAppPath = "";

    public void CheckVersion()
    {
        sJsonStr = "";
        if (!App.AppConfig.UpdateModel)
        {
            StartGame();
        }
        else
        {
            AotUI.Instance.Open("UpdatePanel");
            OnCheckVersion();
        }
    }

//    public async UniTask CheckExtractStreamingAssets()
//    {
//        string version = Application.version;
//        Debug.LogError(PlayerPrefs.GetString("Version", "") + "  |  " + version);
//        if (PlayerPrefs.GetString("Version", "") != version)
//        {
//            await ExtractStreamingAssets();
//            PlayerPrefs.SetString("Version", version);
//            PlayerPrefs.Save();
//        }
//    }

//    async UniTask ExtractStreamingAssets()
//    {
//        if (Directory.Exists(persistentAppPath))
//            Directory.Delete(persistentAppPath, true);
//#if UNITY_ANDROID
//        string infile = $"{Application.streamingAssetsPath}/{zipFile}";
//#elif UNITY_IOS
//        string infile = $"file://{Application.streamingAssetsPath}/{zipFile}";
//#else
//        string infile = $"{Application.streamingAssetsPath}/{zipFile}";
//#endif
//        string outfile = $"{Application.persistentDataPath}/{zipFile}";
//        var data = (await UnityWebRequest.Get(infile).SendWebRequest()).downloadHandler.data;
//        File.WriteAllBytes(outfile, data);
//        ZipHelper.Decompress(outfile, Application.persistentDataPath);
//        File.Delete(outfile);
//        await UniTask.WaitForFixedUpdate();
//    }


    async void OnCheckVersion()
    {
       
    }


    /// <summary>
    /// 转换方法
    /// </summary>
    /// <param name="size">字节值</param>
    /// <returns></returns>
    private string HumanReadableFilesize(double size)
    {
        String[] units = new String[] { "B", "KB", "MB", "GB", "TB", "PB" };
        double mod = 1024.0;
        int i = 0;
        while (size >= mod)
        {
            size /= mod;
            i++;
        }
        return Math.Round(size) + units[i];
    }




    void StartGame()
    {
        AotHybridCLR.Instance.LoadDll();
    }

}

public class DownLoadFile
{
    public string file;
    public string fileInfo;
}
