using System.Collections;
using System.Collections.Generic;
using YooAsset;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class ResManager : Singleton<ResManager>
{

    Dictionary<string, List<AssetHandle>> ResLoaders = new Dictionary<string, List<AssetHandle>>();


    public async UniTask<T> CommonLoadAssetAsync<T>(string location) where T : UnityEngine.Object
    {
        return await LoadAssetAsync<T>("Common", location);
    }

    public async UniTask<T> SceneLoadAssetAsync<T>(string location) where T : UnityEngine.Object
    {
        return await LoadAssetAsync<T>(LoadSceneManager.Instance.CurScene(), location);
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
        var package = YooAssets.GetPackage(App.AppConfig.PackageName);
        AssetHandle handle = package.LoadAssetAsync<T>(location);
        await handle;
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

    public override void Dispose()
    {
        ResLoaders.Clear();
    }
}
