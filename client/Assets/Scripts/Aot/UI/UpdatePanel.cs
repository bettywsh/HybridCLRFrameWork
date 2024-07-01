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
using System.Runtime.ConstrainedExecution;

public class UpdatePanel : BasePanel
{
    ResourcePackage package = YooAssets.GetPackage(AppSettings.AppConfig.PackageName);
    string packageVersion;
    ResourceDownloaderOperation downloader;
    CancellationTokenSource cancelToken = new CancellationTokenSource();
    public override async UniTask OnOpen()
    {
        if (AppSettings.AppConfig.EPlayMode == EPlayMode.EditorSimulateMode)
        {
            StartGame();
        }
        else
        {
            //判断是否是强更
            string bigVersion = await HttpManager.Instance.GetRequest($"{AppSettings.AppConfig.SvrResIp}Android/ver.txt", null);
            if (int.Parse(bigVersion) > AppSettings.AppConfig.AppVersion)
            {
                AotDialog.Instance.ShowDialogOne("警告", "客户端版本过低，请重新下载", () =>
                {
                    Application.OpenURL($"{AppSettings.AppConfig.SvrResIp}Android/{AppSettings.AppConfig.DownloadApkName}");
                });
                return;
            }

            //处理覆盖安装问题
            string verdir = $"{package.GetPackageSandboxRootDirectory()}/{AppSettings.AppConfig.PackageName}";
            string ver = $"{verdir}/ Finish.txt";
            if (!File.Exists(ver))
            {
                CreateVerSionSandbox(verdir, ver);
            }
            else {
                string[] vers = File.ReadAllText(ver).Split('|');
                if (int.Parse(vers[0]) != AppSettings.AppConfig.AppVersion && int.Parse(vers[1]) != AppSettings.AppConfig.ResVersion && int.Parse(vers[2]) != AppSettings.AppConfig.ChannelId)
                {
                    File.Delete(ver);
                    CreateVerSionSandbox(verdir, ver);
                }
            }

            //拉去资源版本判断小版本
            await UpdatePackageVersion();
        }
    }

    void CreateVerSionSandbox(string verdir, string ver) {
        package.ClearPackageSandbox();
        if (!Directory.Exists(verdir))
        {
            Directory.CreateDirectory(verdir);
        }
        File.WriteAllText(ver, $"{AppSettings.AppConfig.AppVersion}|{AppSettings.AppConfig.ResVersion}|{AppSettings.AppConfig.ChannelId}");
    }

    //获取资源版本
    async UniTask UpdatePackageVersion()
    {
        
        var versionOperation = package.UpdatePackageVersionAsync();
        await versionOperation.Task.AsUniTask();
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
        Debug.LogError("包体版本"+package.GetPackageVersion());
        Debug.LogError("远程版本" + versionOperation.PackageVersion);
        if (int.Parse(package.GetPackageVersion()) < int.Parse(versionOperation.PackageVersion))
        {
            //小版本更新
            await UpdatePackageManifest();
        }
        else
        {
            StartGame();
        }        
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
        await downloader;
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

        AotDialog.Instance.ShowDialogOne("警告", $"有新的资源需要下载,大小为{FileSizeString(totalDownloadBytes)}", async () => {
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

    async void StartGame() {
        this.Close();
        await HybridCLRManager.Instance.LoadDll();
    }


    public override void OnClose()
    {
        
    }

}
