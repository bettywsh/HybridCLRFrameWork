using System.Collections;
using System.Collections.Generic;
using YooAsset;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Threading;

public class ResManager : Singleton<ResManager>
{

    Dictionary<string, Dictionary<string, AssetHandle>> ResLoaders = new Dictionary<string, Dictionary<string, AssetHandle>>();

    ResourcePackage package;
    public override async UniTask Init()
    {
        package = YooAssets.GetPackage(AppSettings.AppConfig.PackageName);        
    }

    public string GetVersion()
    {
        return package.GetPackageVersion();
    }

    //同步加载
    public T CommonLoadAsset<T>(string location) where T : UnityEngine.Object
    {
        var handle = package.LoadAssetSync(location);
        T t = (T)handle.AssetObject;
        return t;
    }

    public T SceneLoadAsset<T>(string location, CancellationToken ct = default) where T : UnityEngine.Object
    {
        AssetHandle handle = package.LoadAssetSync<T>(location);
        T t = (T)handle.AssetObject;
        AddResloader(LoadSceneManager.Instance.CurScene(), handle);
        return t;
    }

    //异步加载
    public async UniTask<T> CommonLoadAssetAsync<T>(string location) where T : UnityEngine.Object
    {
        return await LoadAssetAsync<T>("Common", location, default);
    }

    public async UniTask<T> SceneLoadAssetAsync<T>(string location, CancellationToken ct = default) where T : UnityEngine.Object
    {
        return await LoadAssetAsync<T>(LoadSceneManager.Instance.CurScene(), location, ct);
    }

    #region 框架专用

    public SceneHandle LoadSceneAsync(string location)
    {
        return package.LoadSceneAsync(location, LoadSceneMode.Single);
    }
    public TextAsset LoadAsset<T>(string location) where T : UnityEngine.Object
    {
        AssetHandle ah = package.LoadAssetSync<T>(location);
        package.TryUnloadUnusedAsset(location);
        return ah.AssetObject as TextAsset;
    }

    public async UniTask LoadAllAssetsAsync<T>(string location) where T : UnityEngine.Object
    {
        await package.LoadAllAssetsAsync<T>(location).Task.AsUniTask();
    }
    #endregion


    #region 资源加载标识
    private void AddResloader(string resName, AssetHandle assetHandle)
    {
        if (resName == "Common") return;
        ResLoaders.TryGetValue(resName, out var assetHandles);
        if (assetHandles == null)
        {
            assetHandles = new Dictionary<string, AssetHandle>();
        }
        if (assetHandles.ContainsKey(assetHandle.GetAssetInfo().AssetPath))
        {
            assetHandles.Add(assetHandle.GetAssetInfo().AssetPath, assetHandle);
        }
    }

    private async UniTask<T> LoadAssetAsync<T>(string resName, string location, CancellationToken ct) where T : UnityEngine.Object
    {
        AssetHandle handle = package.LoadAssetAsync<T>(location);
        await handle.WithCancellation(ct).SuppressCancellationThrow();
        T t = (T)handle.AssetObject;
        AddResloader(resName, handle);
        return t;
    }

    public void UnLoadAssetBundle(string resLoaderName)
    {
        if (!ResLoaders.TryGetValue(resLoaderName, out var assetHandles))
        {
            return;
        }
        foreach (var assetHandle in assetHandles)
        {
            assetHandle.Value.Release();
        }
        ResLoaders.Remove(resLoaderName);
    }
    #endregion

    public void GC()
    {
        var package = YooAssets.GetPackage(AppSettings.AppConfig.PackageName);
        var operation = package.UnloadUnusedAssetsAsync();
        //operation.WaitForAsyncComplete(); //支持同步操作
    }

    public override void Dispose()
    {
        //YooAssets.DestroyPackage(AppSettings.AppConfig.PackageName);
        //YooAssets.Destroy();
        ResLoaders.Clear();
    }
}
