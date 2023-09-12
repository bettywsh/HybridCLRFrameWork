using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Threading;

public class AotUpdate : AotMonoSingleton<AotUpdate>
{
    private string sJsonStr = "";
    List<DownLoadFile> needUpdate = new List<DownLoadFile>();
    private string writeClientFiles = "";
    string persistentAppPath = "";
    string zipFile = $"{ResConst.RootFolderName.ToLower()}.zip";

    public void CheckVersion()
    {
        persistentAppPath = $"{Application.persistentDataPath}/{ResConst.RootFolderName.ToLower()}";

        if (!ResConst.UpdateModel)
        {
            StartGame();
        }
        else
        {
            AotUI.Instance.Open("UpdatePanel");
            OnCheckVersion();
        }
    }

    public async UniTask CheckExtractStreamingAssets()
    {
        string version = Application.version;
        Debug.LogError(PlayerPrefs.GetString("Version", "") + "  |  " + version);
        if (PlayerPrefs.GetString("Version", "") != version)
        {
            await ExtractStreamingAssets();
            PlayerPrefs.SetString("Version", version);
            PlayerPrefs.Save();
        }
    }

    async UniTask ExtractStreamingAssets()
    {
        if (Directory.Exists(persistentAppPath))
            Directory.Delete(persistentAppPath, true);
#if UNITY_ANDROID
        string infile = $"{Application.streamingAssetsPath}/{zipFile}";
#elif UNITY_IOS
        string infile = $"file://{Application.streamingAssetsPath}/{zipFile}";
#else
        string infile = $"{Application.streamingAssetsPath}/{zipFile}";
#endif
        string outfile = $"{Application.persistentDataPath}/{zipFile}";
        var data = (await UnityWebRequest.Get(infile).SendWebRequest()).downloadHandler.data;
        File.WriteAllBytes(outfile, data);
        ZipHelper.Decompress(outfile, Application.persistentDataPath);
        Debug.LogError("解压");
        File.Delete(outfile);
        await UniTask.WaitForEndOfFrame(this);
    }

    private string GetFilePath(string fileName)
    {
        string serverHttpFile = "";
#if UNITY_ANDROID
        serverHttpFile = $"{ResConst.SvrResIp}Android/{ResConst.RootFolderName.ToLower()}/";
#elif UNITY_IOS
        serverHttpFile = $"{AotResConst.SvrResIp}Ios/{AotResConst.RootFolderName.ToLower()}/";
#else
        serverHttpFile = $"{AotResConst.SvrResIp}Pc/{AotResConst.RootFolderName.ToLower()}/";        
#endif
        return serverHttpFile + fileName;
    }

    async void OnCheckVersion()
    {
        AotMessage.Instance.MessageNotify(AotMessageConst.Msg_UpdateCheckVersion);
        string serverHttpFile = GetFilePath(ResConst.VerFile);
        JsonData jsonDataInfoServer, jsonDataInfoClient;
        int[] serverVersion = new int[3];
        string downloadHandlerText = null;
        try
        {
            downloadHandlerText = (await UnityWebRequest.Get(serverHttpFile).SendWebRequest()).downloadHandler.text;
        }
        catch
        {
            AotMessage.Instance.MessageNotify(AotMessageConst.Msg_UpdateLostConnect);
            return;
        }
        jsonDataInfoServer = JsonMapper.ToObject(downloadHandlerText);
        serverVersion[0] = (int)jsonDataInfoServer["MainVersion"];
        serverVersion[1] = (int)jsonDataInfoServer["PatchVersion"];
        serverVersion[2] = (int)jsonDataInfoServer["ResVersion"];
        

        int[] clientVersion = new int[3];
        jsonDataInfoClient = JsonMapper.ToObject(LocalFile(ResConst.VerFile));
        clientVersion[0] = (int)jsonDataInfoClient["MainVersion"];
        clientVersion[1] = (int)jsonDataInfoClient["PatchVersion"];
        clientVersion[2] = (int)jsonDataInfoClient["ResVersion"];

        if (serverVersion[0] > clientVersion[0])
        {
            //大版本更新
            AotMessage.Instance.MessageNotify(AotMessageConst.Msg_UpdateBigVersion, GetDownloadURLFromJSON(jsonDataInfoServer));
            return;
        }
        else if (serverVersion[2] > clientVersion[2])
        {
            //小版本更新
            TotalDownloadSize(true);
}
        else
        {
            //不需要更新
            StartGame();
        }
    }

    //通过JSON的解析获取到下载的URL内容
    public string GetDownloadURLFromJSON(JsonData jsonDataInfo)
    {
#if UNITY_ANDROID
        return (string)jsonDataInfo["AndroidUrl"];
#elif UNITY_IOS
        return (string)jsonDataInfo["IosUrl"];
#else
        return (string)jsonDataInfo["PcUrl"];
#endif

    }

    //计算所需要下载的时间
    async void TotalDownloadSize(bool isShowDialog)
    {
        string serverHttpFile = GetFilePath(ResConst.CheckFile);
        string serverFile = (await UnityWebRequest.Get(serverHttpFile).SendWebRequest()).downloadHandler.text;
        if (serverFile == "")
        {
            AotMessage.Instance.MessageNotify(AotMessageConst.Msg_UpdateLostConnect);
            return;
        }
        string clientFile = LocalFile(ResConst.CheckFile);
        string[] serverFiles = serverFile.Split('\n');
        string[] clientFiles = clientFile.Split('\n');
        Dictionary<string, string> cFiles = new Dictionary<string, string>();
        for (int i = 0; i < clientFiles.Length; i++)
        {
            if (clientFiles[i] != "")
            {
                string[] cFile = clientFiles[i].Split('|');
                cFiles.Add(cFile[0], clientFiles[i]);
            }
        }

        double downloadSize = 0;
        needUpdate.Clear();
        writeClientFiles = "";
        for (int i = 0; i < serverFiles.Length; i++)
        {
            if (serverFiles[i] == "")
                continue;
            string[] sFile = serverFiles[i].Split('|');
            string file = null;
            cFiles.TryGetValue(sFile[0], out file);
            if (file == null)
            {
                //新增
                downloadSize += int.Parse(sFile[1]);
                DownLoadFile dlf = new DownLoadFile();
                dlf.file = sFile[0];
                dlf.fileInfo = serverFiles[i];
                needUpdate.Add(dlf);
            }
            else
            {
                if (sFile[2] != cFiles[sFile[0]].Split('|')[2])
                {
                    //修改
                    downloadSize += int.Parse(sFile[1]);
                    DownLoadFile dlf = new DownLoadFile();
                    dlf.file = sFile[0];
                    dlf.fileInfo = serverFiles[i];
                    needUpdate.Add(dlf);
                }
                else
                {
                    writeClientFiles += serverFiles[i] + "\n";
                }
            }
        }
        if (isShowDialog)
        {
            AotMessage.Instance.MessageNotify(AotMessageConst.Msg_UpdateSmallVersion, HumanReadableFilesize(downloadSize));
        }
        else
        {
            OnDownLoadFiles();
        }
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


    public void DownLoadFiles()
    {
        if (needUpdate.Count <= 0)
        {
            StartGame();
        }
        OnDownLoadFiles();
    }

    async void OnDownLoadFiles()
    {
        for (int i = 0; i < needUpdate.Count; i++)
        {
            Debug.LogError(needUpdate[i]);
            string url = GetFilePath(needUpdate[i].file);
            byte[] data = (await UnityWebRequest.Get(url).SendWebRequest()).downloadHandler.data;
            if (data == null)
            {
                AotMessage.Instance.MessageNotify(AotMessageConst.Msg_UpdateLostConnect);
                return;
            }
            File.WriteAllBytes($"{persistentAppPath}/{needUpdate[i].file}", data);
            writeClientFiles += needUpdate[i].fileInfo + "\n";
            File.WriteAllText($"{persistentAppPath}/{ResConst.CheckFile}", writeClientFiles);
        }

        File.WriteAllText($"{persistentAppPath}/{ResConst.VerFile}", sJsonStr, new System.Text.UTF8Encoding(false));
        StartGame();
    }

    string LocalFile(string file)
    {
        string path = $"{persistentAppPath}/{file}";
        return File.ReadAllText(path);
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
