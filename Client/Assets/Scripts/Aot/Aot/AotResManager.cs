using System.Collections;
using System.Collections.Generic;
using YooAsset;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AotResManager : AotSingleton<AotResManager>
{
    ResourcePackage package;
    Dictionary<string, AssetHandle> ResLoaders = new Dictionary<string, AssetHandle>();
    public bool InitSucceed { get; private set; } = true;
    public override async UniTask Init()
    {
        // 初始化资源系统
        YooAssets.Initialize();

        // 创建默认的资源包
        package = YooAssets.TryGetPackage(AppSettings.AppConfig.PackageName);
        if (package == null)
            package = YooAssets.CreatePackage(AppSettings.AppConfig.PackageName);

        // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
        YooAssets.SetDefaultPackage(package);

        EPlayMode ePlayMode = AppSettings.AppConfig.EPlayMode;
#if !UNITY_EDITOR
        if (ePlayMode == EPlayMode.EditorSimulateMode)
        {
            Debug.LogError("EditorSimulateMode is editor-only, fallback to OfflinePlayMode");
            ePlayMode = EPlayMode.OfflinePlayMode;
        }
#endif

        InitializationOperation initializationOperation = null;
        switch (ePlayMode)
        {
#if UNITY_EDITOR
            case EPlayMode.EditorSimulateMode:
                {
                    var buildResult = EditorSimulateModeHelper.SimulateBuild(AppSettings.AppConfig.PackageName);
                    var packageRoot = buildResult.PackageRootDirectory;
                    var createParameters = new EditorSimulateModeParameters();
                    createParameters.EditorFileSystemParameters = FileSystemParameters.CreateDefaultEditorFileSystemParameters(packageRoot);
                    initializationOperation = package.InitializeAsync(createParameters);
                    await initializationOperation.Task.AsUniTask();
                    break;
                }
#endif
            case EPlayMode.OfflinePlayMode:
                {
                    var createParameters = new OfflinePlayModeParameters();
                    createParameters.BuildinFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
                    initializationOperation = package.InitializeAsync(createParameters);
                    await initializationOperation.Task.AsUniTask();
                    break;
                }
            case EPlayMode.HostPlayMode:
                {
                    string defaultHostServer = GetHostServerURL();
                    string fallbackHostServer = GetHostServerURL();
                    IRemoteServices remoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
                    var createParameters = new HostPlayModeParameters();
                    createParameters.BuildinFileSystemParameters = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();
                    createParameters.CacheFileSystemParameters = FileSystemParameters.CreateDefaultCacheFileSystemParameters(remoteServices);
                    createParameters.CacheFileSystemParameters.AddParameter(FileSystemParametersDefine.INSTALL_CLEAR_MODE, EOverwriteInstallClearMode.ClearAllCacheFiles);
                    initializationOperation = package.InitializeAsync(createParameters);
                    await initializationOperation.Task.AsUniTask();
                    break;
                }
            case EPlayMode.WebPlayMode:
            {
                #if UNITY_WEBGL && WEIXINMINIGAME && !UNITY_EDITOR
                var createParameters = new WebPlayModeParameters();
                string defaultHostServer = GetHostServerURL();
                string fallbackHostServer = GetHostServerURL();
                string packageRoot = $"{WeChatWASM.WX.env.USER_DATA_PATH}/__GAME_FILE_CACHE"; //注意：如果有子目录，请修改此处！
                IRemoteServices remoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
                createParameters.WebServerFileSystemParameters = WechatFileSystemCreater.CreateFileSystemParameters(packageRoot, remoteServices);
                initializationOperation = package.InitializeAsync(createParameters);
                #else
                var createParameters = new WebPlayModeParameters();
                createParameters.WebServerFileSystemParameters = FileSystemParameters.CreateDefaultWebServerFileSystemParameters();
                initializationOperation = package.InitializeAsync(createParameters);
                #endif
                await initializationOperation.Task.AsUniTask();
                break;
            }
        }

        // 如果初始化失败弹出提示界面
        if (initializationOperation == null || initializationOperation.Status != EOperationStatus.Succeed)
        {
            await FailAndQuit("资源初始化失败，请退出后重试", initializationOperation == null ? "YooAsset initialize failed" : initializationOperation.Error);
            return;
        }

        var operationPackageVersion = package.RequestPackageVersionAsync();
        await operationPackageVersion.Task.AsUniTask();
        if (operationPackageVersion.Status != EOperationStatus.Succeed)
        {
            await FailAndQuit("获取资源版本失败，请退出后重试", operationPackageVersion.Error);
            return;
        }

        var operationPackageManifest = package.UpdatePackageManifestAsync(operationPackageVersion.PackageVersion);
        await operationPackageManifest.Task.AsUniTask();
        if (operationPackageManifest.Status != EOperationStatus.Succeed)
        {
            await FailAndQuit("更新资源清单失败，请退出后重试", operationPackageManifest.Error);
            return;
        }
    }

    private async UniTask FailAndQuit(string userMsg, string error)
    {
        InitSucceed = false;
        Debug.LogError(error);
        await AotUIManager.Instance.Init();
        var tcs = new UniTaskCompletionSource();
        await AotDialogManager.Instance.ShowDialogOne("提示", userMsg, () => tcs.TrySetResult());
        await tcs.Task;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    public static string GetPlatformFolder()
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
            return "Android";
        if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
            return "IPhone";
        if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
            return "WebGL";
        return "PC";
#else
        if (Application.platform == RuntimePlatform.Android)
            return "Android";
        if (Application.platform == RuntimePlatform.IPhonePlayer)
            return "IPhone";
        if (Application.platform == RuntimePlatform.WebGLPlayer)
            return "WebGL";
        return "PC";
#endif
    }

    public string GetHostServerURL()
    {
        return $"{AppSettings.AppConfig.SvrResIp}{GetPlatformFolder()}/Hotfix";
    }

    public string GetForceUpdateUrl()
    {
        return $"{AppSettings.AppConfig.SvrResIp}{GetPlatformFolder()}/Apk/{AppSettings.AppConfig.DownloadApkName}";
    }


    #region 框架专用
    //public async UniTask LoadSceneAsync(string location)
    //{
    //    await package.LoadSceneAsync(location, LoadSceneMode.Single, false).Task.AsUniTask();
    //}
    public async UniTask<T> LoadAsset<T>(string location) where T : UnityEngine.Object
    {
        AssetHandle ah = package.LoadAssetSync<T>(location);
        await ah.Task.AsUniTask();
        T t = (T)ah.AssetObject;
        AddResloader(location, ah);
        return t;
    }
    #endregion

    private void AddResloader(string resName, AssetHandle assetHandle)
    {
        if (assetHandle == null)
            return;
        if (resName == "Common")
            return;
        if (ResLoaders.ContainsKey(resName))
        {
            assetHandle.Release();
            return;
        }
        ResLoaders.Add(resName, assetHandle);
    }

    public void UnLoadAllAssetBundle()
    {
        foreach ((var _,var assetHandle) in ResLoaders)
        {
            assetHandle.Release();
        }
        ResLoaders.Clear();
    }


    public override void Dispose()
    {
        //package.ForceUnloadAllAssets();
        //YooAssets.DestroyPackage(AppSettings.AppConfig.PackageName);
        //YooAssets.Destroy();
        UnLoadAllAssetBundle();
        System.GC.Collect();
        base.Dispose();  
    }


    /// <summary>
    /// 远端资源地址查询服务类
    /// </summary>
    private class RemoteServices : IRemoteServices
    {
        private readonly string _defaultHostServer;
        private readonly string _fallbackHostServer;

        public RemoteServices(string defaultHostServer, string fallbackHostServer)
        {
            _defaultHostServer = defaultHostServer;
            _fallbackHostServer = fallbackHostServer;
        }
        string IRemoteServices.GetRemoteMainURL(string fileName)
        {
            return $"{_defaultHostServer}/{fileName}";
        }
        string IRemoteServices.GetRemoteFallbackURL(string fileName)
        {
            return $"{_fallbackHostServer}/{fileName}";
        }
    }
}


