using System.Collections;
using System.Collections.Generic;
using YooAsset;
using Cysharp.Threading.Tasks;
using UnityEngine;
using dnlib.PE;

public class AotResManager : AotSingleton<AotResManager>
{
    ResourcePackage package;
    Dictionary<string, AssetHandle> ResLoaders = new Dictionary<string, AssetHandle>();
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

        // 编辑器下的模拟模式
        InitializationOperation initializationOperation = null;
        // 编辑器下的模拟模式
        switch (ePlayMode)
        {
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
   
        }

        // 如果初始化失败弹出提示界面
        if (initializationOperation.Status != EOperationStatus.Succeed)
        {
            Debug.LogWarning($"{initializationOperation.Error}");
        }

        var operationPackageVersion = package.RequestPackageVersionAsync();
        await operationPackageVersion.Task.AsUniTask();
        if (operationPackageVersion.Status != EOperationStatus.Succeed)
        {
            //更新失败
            Debug.LogError(operationPackageVersion.Error);
        }

        var operationPackageManifest = package.UpdatePackageManifestAsync(operationPackageVersion.PackageVersion);
        await operationPackageManifest.Task.AsUniTask();
        if (operationPackageManifest.Status != EOperationStatus.Succeed)
        {
            Debug.LogError(operationPackageManifest.Error);
        }
    }


    string GetHostServerURL()
    {
        string hostServerIP = AppSettings.AppConfig.SvrResIp;
        string appVersion = $"Hotfix";

#if UNITY_EDITOR
        if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
            return $"{hostServerIP}Android/{appVersion}";
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
            return $"{hostServerIP}IPhone/{appVersion}";
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
            return $"{hostServerIP}WebGL/{appVersion}";
        else
            return $"{hostServerIP}PC/{appVersion}";
#else
		        if (Application.platform == RuntimePlatform.Android)
		        	return $"{hostServerIP}Android/{appVersion}";
		        else if (Application.platform == RuntimePlatform.IPhonePlayer)
		        	return $"{hostServerIP}IPhone/{appVersion}";
		        else if (Application.platform == RuntimePlatform.WebGLPlayer)
		        	return $"{hostServerIP}WebGL/{appVersion}";
		        else
		        	return $"{hostServerIP}PC/{appVersion}";
#endif
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
        if (resName == "Common") return;
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


