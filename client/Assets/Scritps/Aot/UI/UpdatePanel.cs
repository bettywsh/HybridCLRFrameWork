using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using YooAsset;
using TMPro;
using UnityEngine.UI;

public class UpdatePanel : BasePanel
{
    ResourcePackage package;
    string packageVersion;
    ResourceDownloaderOperation downloader;
    CancellationTokenSource cancelToken = new CancellationTokenSource();
    public override async void OnOpen()
    {
        if (AppSettings.AppConfig.EPlayMode == EPlayMode.EditorSimulateMode)
        {
            StartGame();
        }
        else
        {
            await UpdatePackageVersion();
        }
    }

    //获取资源版本
    async UniTask UpdatePackageVersion()
    {
        package = YooAssets.GetPackage(AppSettings.AppConfig.PackageName);
        var versionOperation = package.UpdatePackageVersionAsync();
        await versionOperation;
        if (versionOperation.Status == EOperationStatus.Succeed)
        {
            //更新成功
            packageVersion = versionOperation.PackageVersion;
            Debug.Log($"Updated package Version : {packageVersion}");
        }
        else
        {
            AotDialog.Instance.ShowDialogOne("警告", "获取资源版本失败，请检查网络", async () => {
                await UpdatePackageVersion();
            });
            //更新失败
            Debug.LogError(versionOperation.Error);
            return;
        }
        await UpdatePackageManifest();
    }

    //获取资源列表
    async UniTask UpdatePackageManifest()
    {
        var manifestOperation = package.UpdatePackageManifestAsync(packageVersion, true);
        await manifestOperation;

        if (manifestOperation.Status != EOperationStatus.Succeed)
        {
            AotDialog.Instance.ShowDialogOne("警告", "更新资源清单失败，请检查网络", async () => {
                await UpdatePackageManifest();
            });
            //更新失败
            Debug.LogError(manifestOperation.Error);
            return;
        }
        await CreateDownloader();
    }

    public async UniTask CreateDownloader()
    {
        int downloadingMaxNum = 10;
        int failedTryAgain = 3;
        downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);

        //没有需要下载的资源
        if (downloader.TotalDownloadCount == 0)
        {
            StartGame();
            return;
        }

        //需要下载的文件总数和总大小
        int totalDownloadCount = downloader.TotalDownloadCount;
        long totalDownloadBytes = downloader.TotalDownloadBytes;

        //注册回调方法
        downloader.OnDownloadErrorCallback = OnDownloadErrorFunction;
        downloader.OnDownloadProgressCallback = OnDownloadProgressUpdateFunction;
        //downloader.OnDownloadOverCallback = OnDownloadOverFunction;
        //downloader.OnStartDownloadFileCallback = OnStartDownloadFileFunction;

        AotDialog.Instance.ShowDialogOne("警告", $"有新的资源需要下载,大小为{HumanReadableFilesize(totalDownloadBytes)}", async () => {
            await Download();
        });
    }

    public async UniTask Download()
    {
        //开启下载
        downloader.BeginDownload();
        await downloader.Task.AsUniTask();

        //检测下载结果
        if (downloader.Status == EOperationStatus.Succeed)
        {
            await package.ClearUnusedCacheFilesAsync();
            //下载成功
            StartGame();
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


    void OnDownloadErrorFunction(string fileName, string error)
    {
        AotDialog.Instance.ShowDialogOne("警告", "下载文件失败，是否重新下载", async () => {
            downloader.CancelDownload();
            await Download();        
        });
    }

    void OnDownloadProgressUpdateFunction(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes)
    {
        referenceData["txtContent"].tmptxtValue.text = $"下载最新资源({currentDownloadCount})/({totalDownloadCount})";
        referenceData["imgProgress"].imgValue.fillAmount = currentDownloadBytes / totalDownloadBytes;

    }

    //void OnDownloadOverFunction(bool isSucceed)
    //{

    //}

    //void OnStartDownloadFileFunction(string fileName, long sizeBytes)
    //{ 

    //}

    void StartGame() {
        UIManager.Instance.Close<UpdatePanel>();
        HybridCLRManager.Instance.LoadDll();
    }


    public override void OnClose()
    {
        
    }

}
