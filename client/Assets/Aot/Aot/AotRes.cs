using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public class AotRes : Singleton<AotRes>
{
    Dictionary<string, AssetHandle> abDic = new Dictionary<string, AssetHandle>();

    public override async UniTask InitUniTask()
    {
        string packageName = "DefaultPackage";
        // 初始化资源系统
        YooAssets.Initialize();

        // 创建默认的资源包
        var package = YooAssets.CreatePackage(packageName);

        // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
        YooAssets.SetDefaultPackage(package);

        EPlayMode ePlayMode = App.AppConfig.EPlayMode;

        // 编辑器下的模拟模式
        InitializationOperation initializationOperation = null;
        // 编辑器下的模拟模式
        switch (ePlayMode)
        {
            case EPlayMode.EditorSimulateMode:
                {
                    EditorSimulateModeParameters createParameters = new();
                    createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(EDefaultBuildPipeline.BuiltinBuildPipeline.ToString(), packageName);
                    initializationOperation = package.InitializeAsync(createParameters);
                    await initializationOperation.Task.AsUniTask();
                    break;
                }
            case EPlayMode.OfflinePlayMode:
                {
                    OfflinePlayModeParameters createParameters = new();
                    initializationOperation = package.InitializeAsync(createParameters);
                    await initializationOperation.Task.AsUniTask();
                    break;
                }
            case EPlayMode.HostPlayMode:
                {
                    string defaultHostServer = GetHostServerURL();
                    string fallbackHostServer = GetHostServerURL();
                    HostPlayModeParameters createParameters = new();
                    createParameters.DecryptionServices = new FileStreamDecryption();
                    createParameters.BuildinQueryServices = new GameQueryServices();
                    createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
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
        string hostServerIP = App.AppConfig.SvrResIp;
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

    public async UniTask<T> LoadAssetAsync<T>(string location) where T : UnityEngine.Object
    {
        AssetHandle handle = YooAssets.LoadAssetAsync<T>(location);
        await handle.Task.AsUniTask();
        T t = (T)handle.AssetObject;
        abDic.Add(location, handle);
        return t;
    }

    public void UnLoadAssetAsync()
    {
        foreach ((string path, AssetHandle assetHandle) in abDic)
        {
            assetHandle.Release();
        }
        abDic.Clear();

    }



}
