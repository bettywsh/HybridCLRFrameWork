using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using YooAsset;
using TMPro;
using UnityEngine.UI;
using System.IO;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

public class UpdatePanel : AotPanelBase
{
    public TextMeshProUGUI txtContent;
    public Image imgProgress;

    ResourcePackage package;
    string packageVersion;
    ResourceDownloaderOperation downloader;
    TweenerCore<float, float, FloatOptions> tween;
    public override async UniTask OnOpen()
    {
        if (AppSettings.AppConfig.EPlayMode == EPlayMode.EditorSimulateMode ||
         AppSettings.AppConfig.EPlayMode == EPlayMode.OfflinePlayMode ||
         AppSettings.AppConfig.EPlayMode == EPlayMode.WebPlayMode)
        {
            txtContent.text = "检查游戏资源";
            StartGame();
        }
        else
        {
            txtContent.text = "获取版本";
            SetProgressTween(true);
            package = YooAssets.GetPackage(AppSettings.AppConfig.PackageName);
            // Check whether a force update is required
            string bigVersion = await AotHttpManager.Instance.GetRequest($"{AotResManager.Instance.GetHostServerURL()}/ver.txt", null);
            if (bigVersion == "")
            {
                SetProgressTween(false);
                await AotDialogManager.Instance.ShowDialogOne("提示", "获取资源版本失败，请重试", () => {
                    OnOpen();
                });
                return;
            }
            if (int.Parse(bigVersion) > AppSettings.AppConfig.AppVersion)
            {
                SetProgressTween(false);
                await AotDialogManager.Instance.ShowDialogOne("提示", "客户端版本过低，请下载安装新版本", () =>
                {
                    Application.OpenURL(AotResManager.Instance.GetForceUpdateUrl());
                    OnOpen();
                });
                return;
            }

            await CreateDownloader();
        }
    }


    public async UniTask CreateDownloader()
    {
        int downloadingMaxNum = 10;
        int failedTryAgain = 3;
        downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
        //await downloader;
        // No resource files need downloading
        SetProgressTween(false);
        if (downloader.TotalDownloadCount == 0)
        {
            StartGame();
            return;
        }

        // Total file count and byte size to download
        int totalDownloadCount = downloader.TotalDownloadCount;
        long totalDownloadBytes = downloader.TotalDownloadBytes;

        // Register callbacks
        downloader.DownloadErrorCallback = OnDownloadErrorFunction;
        downloader.DownloadUpdateCallback = OnDownloadProgressUpdateFunction;
        //downloader.OnDownloadOverCallback = OnDownloadOverFunction;
        //downloader.OnStartDownloadFileCallback = OnStartDownloadFileFunction;

        //AotDialogManager.Instance.ShowDialogOne("提示", $"检测到资源需要下载，大小为{FileSizeString(totalDownloadBytes)}", async () => {
        //    await Download();
        //});
        await Download();
    }

    public async UniTask Download()
    {
        downloader.BeginDownload();
        await downloader.Task.AsUniTask();

        if (downloader.Status != EOperationStatus.Succeed)
        {
            SetProgressTween(false);
            await AotDialogManager.Instance.ShowDialogOne("提示", "下载文件失败，请重试", () =>
            {
                CreateDownloader().Forget();
            });
            return;
        }

        await ClearCacheAndStart();
    }

    async UniTask ClearCacheAndStart()
    {
        var operation = package.ClearCacheFilesAsync(EFileClearMode.ClearUnusedBundleFiles);
        await operation.Task.AsUniTask();
        if (operation.Status != EOperationStatus.Succeed)
        {
            SetProgressTween(false);
            await AotDialogManager.Instance.ShowDialogOne("提示", "清理缓存失败，请重试", () =>
            {
                ClearCacheAndStart().Forget();
            });
            return;
        }
        StartGame().Forget();
    }

    /// <summary>
    /// Convert file size unit
    /// </summary>
    /// <param name="size">Byte size</param>
    /// <returns></returns>
    private string FileSizeString(double size)
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


    void OnDownloadErrorFunction(DownloadErrorData downloadErrorData)
    {
        Debug.LogError($"download error: {downloadErrorData}");
    }
    
    void OnDownloadProgressUpdateFunction(DownloadUpdateData downloadUpdateData)
    {
        txtContent.text = $"正在下载资源{downloadUpdateData.CurrentDownloadCount}/{downloadUpdateData.TotalDownloadCount}";
        imgProgress.fillAmount = downloadUpdateData.CurrentDownloadBytes / (downloadUpdateData.TotalDownloadBytes * 1.0f);
    }

    public void SetTitle(string tilte)
    {
        txtContent.text = tilte;
    }

    public void SetProgressTween(bool isProgrss)
    {
        if (isProgrss)
        {
            if (tween != null)
            {
                tween.Kill(true);
            }
            imgProgress.fillAmount = 0;
            tween = imgProgress.DOFillAmount(1, 6.0f);
        }
        else
        {
            if (tween != null)
            {
                tween.Kill(true);
            }
        }
        
    }

    async UniTask StartGame() {
        //this.Close();
        imgProgress.fillAmount = 1.0f;
        await HybridCLRManager.Instance.LoadDll();
    }


}
