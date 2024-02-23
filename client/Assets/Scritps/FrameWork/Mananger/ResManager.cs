using System.Collections;
using System.Collections.Generic;
using YooAsset;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ResManager : Singleton<ResManager>
{

    Dictionary<string, List<AssetHandle>> ResLoaders = new Dictionary<string, List<AssetHandle>>();

    ResourcePackage package;
    public override async UniTask InitUniTask()
    {
        // 初始化资源系统
        YooAssets.Initialize();

        // 创建默认的资源包
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
                    EditorSimulateModeParameters createParameters = new EditorSimulateModeParameters();
                    createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline.ToString(), AppSettings.AppConfig.PackageName);
                    initializationOperation = package.InitializeAsync(createParameters);
                    await initializationOperation.Task.AsUniTask();
                    break;
                }
            case EPlayMode.OfflinePlayMode:
                {
                    OfflinePlayModeParameters createParameters = new OfflinePlayModeParameters();
                    initializationOperation = package.InitializeAsync(createParameters);
                    await initializationOperation.Task.AsUniTask();
                    break;
                }
            case EPlayMode.HostPlayMode:
                {
                    string defaultHostServer = GetHostServerURL();
                    string fallbackHostServer = GetHostServerURL();
                    HostPlayModeParameters createParameters = new HostPlayModeParameters();
                    //createParameters.DecryptionServices = new FileStreamDecryption();
                    //createParameters.BuildinQueryServices = new GameQueryServices();
                    //createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
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
        else
        {
            var version = package.GetPackageVersion();
            Debug.Log($"Init resource package version : {version}");
        }
    }

    string GetHostServerURL()
    {
        string hostServerIP = AppSettings.AppConfig.SvrResIp;
        string appVersion = $"";

#if UNITY_EDITOR
        if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
            return $"{hostServerIP}/Android/{appVersion}";
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
            return $"{hostServerIP}/IPhone/{appVersion}";
        else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
            return $"{hostServerIP}/WebGL/{appVersion}";
        else
            return $"{hostServerIP}/PC/{appVersion}";
#else
		        if (Application.platform == RuntimePlatform.Android)
		        	return $"{hostServerIP}/Android/{appVersion}";
		        else if (Application.platform == RuntimePlatform.IPhonePlayer)
		        	return $"{hostServerIP}/IPhone/{appVersion}";
		        else if (Application.platform == RuntimePlatform.WebGLPlayer)
		        	return $"{hostServerIP}/WebGL/{appVersion}";
		        else
		        	return $"{hostServerIP}/PC/{appVersion}";
#endif
    }

    public async UniTask<T> CommonLoadAssetAsync<T>(string location) where T : UnityEngine.Object
    {
        return await LoadAssetAsync<T>("Common", location);
    }

    public async UniTask<T> SceneLoadAssetAsync<T>(string location) where T : UnityEngine.Object
    {
        return await LoadAssetAsync<T>(LoadSceneManager.Instance.CurScene(), location);
    }

    public async UniTask LoadSceneAsync(string location)
    {
        await package.LoadSceneAsync(location, LoadSceneMode.Single, false).Task.AsUniTask();
    }


    #region 资源加载标识
    private void AddResloader(string resName, AssetHandle assetHandle)
    {
        if (resName == "Common") return;
        List<AssetHandle> assetHandles = null;
        if (!ResLoaders.TryGetValue(resName, out assetHandles))
        {
            assetHandles = new List<AssetHandle>();
            assetHandles.Add(assetHandle);
            ResLoaders.Add(resName, assetHandles);
        }
        else
        {
            assetHandles.Add(assetHandle);
        }
    }

    private async UniTask<T> LoadAssetAsync<T>(string resName, string location) where T : UnityEngine.Object
    {
        AssetHandle handle = package.LoadAssetAsync<T>(location);
        await handle.Task.AsUniTask();
        T t = (T)handle.AssetObject;
        AddResloader(resName, handle);
        return t;
    }

    public void UnLoadAssetBundle(string resLoaderName)
    {
        List<AssetHandle> assetHandles = null;
        if (!ResLoaders.TryGetValue(resLoaderName, out assetHandles))
        {
            return;
        }
        for (int i = 0; i < assetHandles.Count; i++)
        {
            assetHandles[i].Release();
        }
        ResLoaders.Remove(resLoaderName);
    }
    #endregion

    public void GC()
    {
        var package = YooAssets.GetPackage(AppSettings.AppConfig.PackageName);
        package.UnloadUnusedAssets();
    }

    public override void Dispose()
    {
        ResLoaders.Clear();
    }
}
