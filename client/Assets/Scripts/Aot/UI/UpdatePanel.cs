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
            //�ж��Ƿ���ǿ��
            string bigVersion = await HttpManager.Instance.GetRequest($"{AppSettings.AppConfig.SvrResIp}Android/ver.txt", null);
            if (int.Parse(bigVersion) > AppSettings.AppConfig.AppVersion)
            {
                AotDialog.Instance.ShowDialogOne("����", "�ͻ��˰汾���ͣ�����������", () =>
                {
                    Application.OpenURL($"{AppSettings.AppConfig.SvrResIp}Android/{AppSettings.AppConfig.DownloadApkName}");
                });
                return;
            }

            //�����ǰ�װ����
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

            //��ȥ��Դ�汾�ж�С�汾
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

    //��ȡ��Դ�汾
    async UniTask UpdatePackageVersion()
    {
        
        var versionOperation = package.UpdatePackageVersionAsync();
        await versionOperation.Task.AsUniTask();
        if (versionOperation.Status == EOperationStatus.Succeed)
        {
            //���³ɹ�
            packageVersion = versionOperation.PackageVersion;
            Debug.Log($"Updated package Version : {packageVersion}");
        }
        else
        {
            AotDialog.Instance.ShowDialogOne("����", "��ȡ��Դ�汾ʧ�ܣ���������", async () => {
                await UpdatePackageVersion();
            });
            //����ʧ��
            Debug.LogError(versionOperation.Error);
            return;
        }
        Debug.LogError("����汾"+package.GetPackageVersion());
        Debug.LogError("Զ�̰汾" + versionOperation.PackageVersion);
        if (int.Parse(package.GetPackageVersion()) < int.Parse(versionOperation.PackageVersion))
        {
            //С�汾����
            await UpdatePackageManifest();
        }
        else
        {
            StartGame();
        }        
    }

    //��ȡ��Դ�б�
    async UniTask UpdatePackageManifest()
    {
        var manifestOperation = package.UpdatePackageManifestAsync(packageVersion, true);
        await manifestOperation;

        if (manifestOperation.Status != EOperationStatus.Succeed)
        {
            AotDialog.Instance.ShowDialogOne("����", "������Դ�嵥ʧ�ܣ���������", async () => {
                await UpdatePackageManifest();
            });
            //����ʧ��
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
        //û����Ҫ���ص���Դ
        if (downloader.TotalDownloadCount == 0)
        {
            StartGame();
            return;
        }

        //��Ҫ���ص��ļ��������ܴ�С
        int totalDownloadCount = downloader.TotalDownloadCount;
        long totalDownloadBytes = downloader.TotalDownloadBytes;

        //ע��ص�����
        downloader.OnDownloadErrorCallback = OnDownloadErrorFunction;
        downloader.OnDownloadProgressCallback = OnDownloadProgressUpdateFunction;
        //downloader.OnDownloadOverCallback = OnDownloadOverFunction;
        //downloader.OnStartDownloadFileCallback = OnStartDownloadFileFunction;

        AotDialog.Instance.ShowDialogOne("����", $"���µ���Դ��Ҫ����,��СΪ{FileSizeString(totalDownloadBytes)}", async () => {
            await Download();
        });
    }

    public async UniTask Download()
    {
        //��������
        downloader.BeginDownload();
        await downloader.Task.AsUniTask();

        //������ؽ��
        if (downloader.Status == EOperationStatus.Succeed)
        {
            await package.ClearUnusedCacheFilesAsync();
            //���سɹ�
            StartGame();
        }
    }

    /// <summary>
    /// ת������
    /// </summary>
    /// <param name="size">�ֽ�ֵ</param>
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
        AotDialog.Instance.ShowDialogOne("����", "�����ļ�ʧ�ܣ��Ƿ���������", async () => {
            downloader.CancelDownload();
            await Download();        
        });
    }

    void OnDownloadProgressUpdateFunction(int totalDownloadCount, int currentDownloadCount, long totalDownloadBytes, long currentDownloadBytes)
    {
        referenceData["txtContent"].tmptxtValue.text = $"����������Դ({currentDownloadCount})/({totalDownloadCount})";
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
