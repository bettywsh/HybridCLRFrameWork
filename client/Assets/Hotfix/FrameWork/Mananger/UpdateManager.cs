﻿using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class UpdateManager : MonoSingleton<UpdateManager>
{
    private string sJsonStr = "";
    List<DownLoadFile> needUpdate = new List<DownLoadFile>();
    private string writeClientFiles = "";
    public void CheckVersion()
    {
        if (!AppConst.UpdateModel)
        {
            StartGame();
            return;
        }
        else
        {
            UIManager.Instance.Open("UpdatePanel");
            if (File.Exists(Application.persistentDataPath + "/" + ResConst.RootFolderName))
            {
                StartCoroutine(OnCheckVersion());
            }
            else 
            {                
                StartCoroutine(ExtractStreamingAssetsPath());
            }
        }
    }

    void DeletePersistentDataPath()
    {
        Directory.Delete(Application.persistentDataPath + "/" + ResConst.RootFolderName, true);
    }


    IEnumerator ExtractStreamingAssetsPath()
    {
        MessageManager.Instance.MessageNotify(MessageConst.MsgUpdateFristCopy);
        string rootPath = Application.persistentDataPath + "/" + ResConst.RootFolderName.ToLower();
        if (!Directory.Exists(rootPath))
            Directory.CreateDirectory(rootPath);
        string infile = string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, ResConst.RootFolderName.ToLower(), ResConst.VerFile);
        string outfile = string.Format("{0}/{1}/{2}", Application.persistentDataPath, ResConst.RootFolderName.ToLower(), ResConst.VerFile);
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW www = new WWW(infile);
            yield return www;
            if (www.isDone)
            {
                File.WriteAllBytes(outfile, www.bytes);
            }
        }
        else File.Copy(infile, outfile, true);
        yield return new WaitForEndOfFrame();

        infile = string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, ResConst.RootFolderName.ToLower(), ResConst.CheckFile);
        outfile = string.Format("{0}/{1}/{2}", Application.persistentDataPath, ResConst.RootFolderName.ToLower(), ResConst.CheckFile);
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW www = new WWW(infile);
            yield return www;
            if (www.isDone)
            {
                File.WriteAllBytes(outfile, www.bytes);
            }
        }
        else File.Copy(infile, outfile, true);
        yield return new WaitForEndOfFrame();

        // 释放所有文件到数据目录
        string[] files = File.ReadAllLines(outfile);     
        for (int i =0;i< files.Length;i++)        {
            string[] fs = files[i].Split('|');
            infile = string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, ResConst.RootFolderName.ToLower(), fs[0]);
            outfile = string.Format("{0}/{1}/{2}", Application.persistentDataPath, ResConst.RootFolderName.ToLower(), fs[0]);

            MessageManager.Instance.MessageNotify(MessageConst.MsgUpdateFristProgress, i, files.Length);

            string dir = Path.GetDirectoryName(outfile);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            if (Application.platform == RuntimePlatform.Android)
            {
                WWW www = new WWW(infile);
                yield return www;

                if (www.isDone)
                {
                    File.WriteAllBytes(outfile, www.bytes);
                }
                yield return 0;
            }
            else
            {
                if (File.Exists(outfile))
                {
                    File.Delete(outfile);
                }
                File.Copy(infile, outfile, true);
            }
            yield return new WaitForEndOfFrame();
        }
        //启动版本检测
        StartCoroutine(OnCheckVersion());
    }

    private string GetFilePath(string fileName)
    {
        string serverHttpFile = "";
#if UNITY_ANDROID
        serverHttpFile = string.Format("{0}Android/{1}/", AppConst.SvrResIp, ResConst.RootFolderName.ToLower());
#elif UNITY_IOS
        serverHttpFile = string.Format("{0}IOS/{1}/", AppConst.SvrResIp, ResConst.RootFolderName.ToLower());
#else
        serverHttpFile = string.Format("{0}Pc/{1}/", AppConst.SvrResIp, ResConst.RootFolderName.ToLower());
#endif
        return serverHttpFile + fileName;
    }

    IEnumerator OnCheckVersion()
    {
        MessageManager.Instance.MessageNotify(MessageConst.MsgUpdateCheckVersion);
        string serverHttpFile = GetFilePath(ResConst.VerFile);
        JsonData jsonDataInfoServer, jsonDataInfoClient;
        int[] serverVersion = new int[3];
        UnityWebRequest webRequest = UnityWebRequest.Get(serverHttpFile);
        yield return webRequest.SendWebRequest();
        if (webRequest.isHttpError || webRequest.isNetworkError)
        {
            MessageManager.Instance.MessageNotify(MessageConst.MsgUpdateLostConnect);
            yield break;
        }
        else
        {
            jsonDataInfoServer = JsonMapper.ToObject(webRequest.downloadHandler.text);
            serverVersion[0] = (int)jsonDataInfoServer["MainVersion"];
            serverVersion[1] = (int)jsonDataInfoServer["PatchVersion"];
            serverVersion[2] = (int)jsonDataInfoServer["ResVersion"];
        }

        int[] clientVersion = new int[3];
        LocalText localText = new LocalText();
        yield return StartCoroutine(LocalFile(ResConst.VerFile, localText));
        jsonDataInfoClient = JsonMapper.ToObject(localText.text);
        clientVersion[0] = (int)jsonDataInfoClient["MainVersion"];
        clientVersion[1] = (int)jsonDataInfoClient["PatchVersion"];
        clientVersion[2] = (int)jsonDataInfoClient["ResVersion"];

        //可写文件夹版本太低， 可能覆盖安装了
        //if (int.Parse(serverVersion[2]) > int.Parse(clientVersion[2]))
        //{            
        //    DeletePersistentDataPath();
        //    localText = new LocalText();
        //    LocalFile(ResConst.VerFile, localText);
        //    jsonDataInfoClient = JsonMapper.ToObject(localText.text);
        //    cversion = (string)jsonDataInfoClient["GameVersion"];
        //    cversions = cversion.Split('.');
        //    clientVersion[0] = cversions[0];
        //    clientVersion[1] = cversions[1];
        //    clientVersion[2] = cversions[2];
        //    clientVersion[3] = (string)jsonDataInfoClient["ResVersion"];
        //}
        //AppConst.GameVersion = sVersion;

        if (serverVersion[0] > clientVersion[0])
        {
            //大版本更新
            MessageManager.Instance.MessageNotify(MessageConst.MsgUpdateBigVersion, GetDownloadURLFromJSON(jsonDataInfoServer));
            yield break;
        }
        else if (serverVersion[2] > clientVersion[2])
        {
            //小版本更新
            StartCoroutine(TotalDownloadSize(true));
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
    IEnumerator TotalDownloadSize(bool isShowDialog)
    {
        string serverFile = "";
        string serverHttpFile = GetFilePath(ResConst.CheckFile);
        UnityWebRequest webRequest = UnityWebRequest.Get(serverHttpFile);
        yield return webRequest.SendWebRequest();
        if (webRequest.isHttpError || webRequest.isNetworkError)
        {
            MessageManager.Instance.MessageNotify(MessageConst.MsgUpdateLostConnect);
            yield break;
        }
        else
        {
            serverFile = webRequest.downloadHandler.text;
        }
        LocalText localText = new LocalText();
        yield return StartCoroutine(LocalFile(ResConst.CheckFile, localText));
        string clientFile = localText.text;

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
                if (sFile[3] != cFiles[sFile[0]].Split('|')[3])
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
            MessageManager.Instance.MessageNotify(MessageConst.MsgUpdateSmallVersion, HumanReadableFilesize(downloadSize));
        }
        else
        {
            StartCoroutine(OnDownLoadFiles());
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
        StartCoroutine(OnDownLoadFiles());
    }

    IEnumerator OnDownLoadFiles()
    {
        for (int i = 0; i < needUpdate.Count; i++)
        {
            string url = AppConst.SvrResIp + needUpdate[i].file;
            using (var www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ProtocolError || www.result == UnityWebRequest.Result.ConnectionError)
                {
                    MessageManager.Instance.MessageNotify(MessageConst.MsgUpdateLostConnect);
                    yield break;
                }
                while (!www.isDone)
                {
                    yield return null;
                }
                if (www.isDone)
                {
                    File.WriteAllBytes(needUpdate[i].file, www.downloadHandler.data);
                }
            }
            writeClientFiles += needUpdate[i].fileInfo + "\n";
            File.WriteAllText(string.Format("{0}/{1}", Application.persistentDataPath, ResConst.CheckFile), writeClientFiles);
        }

        
        File.WriteAllText(string.Format("{0}/{1}", Application.persistentDataPath, ResConst.VerFile), sJsonStr, new System.Text.UTF8Encoding(false));
        MessageManager.Instance.MessageNotify(MessageConst.MsgUpdateDownLoadComplete);
    }

    IEnumerator LocalFile(string file, LocalText retText)
    {
        string path = string.Format("{0}/{1}/{2}", Application.persistentDataPath, ResConst.RootFolderName.ToLower(), file);
        if (File.Exists(path))
        {
            retText.text = File.ReadAllText(path);
        }
        else
        {           
#if UNITY_ANDROID
            path = string.Format("{0}/{1}/{2}", Application.streamingAssetsPath, ResConst.RootFolderName.ToLower(), ResConst.CheckFile);
#endif

#if UNITY_IOS
            path = string.Format("file://{0}/{1}/{2}", Application.streamingAssetsPath, ResConst.RootFolderName.ToLower(), ResConst.CheckFile);
#endif


            UnityWebRequest uwr = UnityWebRequest.Get(path);
            yield return uwr.SendWebRequest();
            retText.text = uwr.downloadHandler.text;
        }
    }

    //private void Update()
    //{
    //    if (isProgress)
    //    {
    //        MessageManager.Instance.EventNotify(MessageConst.MsgUpdateDownLoadUpdate, param1, (curDownLoadSize + param2) / allDownLoadSize);
    //    }
    //}

    void StartGame()
    {
        UpdateManager.Instance.Dispose();
        SoundManager.Instance.Dispose();
        UIManager.Instance.Dispose();
        AtlasManager.Instance.Dispose();


        NetworkManager.Instance.Init();
        UIManager.Instance.Open("LoginPanel");
    }

}

public class DownLoadFile
{
    public string file;
    public string fileInfo;
}

public class LocalText
{
    public string text;
}
