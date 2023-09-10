using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

class UnZipInfo
{
    public string zipfile;
    public string outPath;
    public uint fileCount;
}

class UnzipEvent
{
    public string name;
    public uint currValue;

    public UnzipEvent(string name, uint value)
    {
        this.name = name;
        this.currValue = value;
    }
}
public class CZip : MonoBehaviour
{
    uint zipFileCount = 0;
    static uint zipFileIndex = 0;
    Action<float, float> mProgress = null;
    List<UnZipInfo> mZips = new List<UnZipInfo>();
    static Queue<UnzipEvent> zipEvents = new Queue<UnzipEvent>();
    private static CZip uZip = null;
    private static readonly object mZiplock = new object();

    public static CZip Create()
    {
        var gameObj = new GameObject("ZipObject");
        uZip = gameObj.AddComponent<CZip>();
        return uZip;
    }

    void Update()
    {
        lock (mZiplock)
        {
            var batchCount = 5;
            while (zipEvents.Count > 0)
            {
                var ev = zipEvents.Dequeue();
                OnUpdateUnzipProgress(ev);
                if (--batchCount == 0)
                {
                    return;
                }
            }
        }
    }

    private void OnUpdateUnzipProgress(UnzipEvent ev)
    {
        Debug.LogWarning("unzip:>>" + ev.name + " curr:" + ev.currValue + " count:" + zipFileCount);
        if (mProgress != null)
        {
            mProgress(ev.currValue, zipFileCount);
        }
        if (ev.currValue >= zipFileCount)
        {
            this.OnDispose();   //���ٶ�����
        }
    }

    /// <summary>
    /// ѹ���ļ�
    /// </summary>
    /// <param name="zipfile"></param>
    /// <param name="srcDir"></param>
    public static void ZipFile(string zipfile, string srcDir, string fileType = null)
    {
        FastZip zipin = new FastZip();
        zipin.CreateEmptyDirectories = true;
        string fileFilter = null;
        if (fileType != null)
        {
            fileFilter = ".*\\.(" + fileType + ")$";
        }
        zipin.CreateZip(zipfile, srcDir, true, fileFilter);
    }

    /// <summary>
    /// ��ѹ���ļ�
    /// </summary>
    /// <param name="zipfile"></param>
    /// <param name="destDir"></param>
    public void AddUnzip(string zipfile, string destDir, uint fileCount)
    {
        var zip = new UnZipInfo();
        zip.zipfile = zipfile;
        zip.outPath = destDir;
        zip.fileCount = fileCount;
        mZips.Add(zip);
    }

    public void DoUnzip(Action<float, float> onProgress)
    {
        mProgress = onProgress;
        zipFileIndex = zipFileCount = 0;

        foreach (var zip in mZips)
        {
            zipFileCount += zip.fileCount;
        }
        foreach (var zip in mZips)
        {
            var events = new FastZipEvents();
            events.CompletedFile = ProcessFileMethod;

            var zipout = new FastZip(events);
            zipout.ExtractZip(zip.zipfile, zip.outPath, null);
        }
    }

    private void ProcessFileMethod(object sender, ScanEventArgs e)
    {
        lock (mZiplock)
        {
            zipEvents.Enqueue(new UnzipEvent(e.Name, ++zipFileIndex));
        }
    }

    public void OnDispose()
    {
        mZips.Clear();
        if (uZip != null)
        {
            GameObject.Destroy(uZip.gameObject);
        }
        uZip = null;
    }
}