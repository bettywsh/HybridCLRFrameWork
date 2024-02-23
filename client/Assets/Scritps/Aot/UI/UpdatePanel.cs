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

    //��ȡ��Դ�汾
    async UniTask UpdatePackageVersion()
    {
        package = YooAssets.GetPackage(AppSettings.AppConfig.PackageName);
        var versionOperation = package.UpdatePackageVersionAsync();
        await versionOperation;
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
        await UpdatePackageManifest();
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

        AotDialog.Instance.ShowDialogOne("����", $"���µ���Դ��Ҫ����,��СΪ{HumanReadableFilesize(totalDownloadBytes)}", async () => {
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

    void StartGame() {
        UIManager.Instance.Close<UpdatePanel>();
        HybridCLRManager.Instance.LoadDll();
    }


    public override void OnClose()
    {
        
    }

}
