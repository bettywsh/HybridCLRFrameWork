using System.Collections;
using System.Collections.Generic;
using YooAsset;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AotResManager : AotSingleton<AotResManager>
{
    ResourcePackage package;

    public override async UniTask Init()
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

    #region 框架专用
    public async UniTask LoadSceneAsync(string location)
    {
        await package.LoadSceneAsync(location, LoadSceneMode.Single, false).Task.AsUniTask();
    }
    public async UniTask<T> LoadAsset<T>(string location) where T : UnityEngine.Object
    {
        AssetHandle ah = package.LoadAssetSync<T>(location);
        await ah.Task.AsUniTask();
        T t = (T)ah.AssetObject;
        return t;
    }
    #endregion



    public override void Dispose()
    {
        package.ForceUnloadAllAssets();
        YooAssets.DestroyPackage(AppSettings.AppConfig.PackageName);
        YooAssets.Destroy();
        System.GC.Collect();
        base.Dispose();  
    }

}
