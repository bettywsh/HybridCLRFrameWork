﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UObject = UnityEngine.Object;
using static UnityEngine.Networking.UnityWebRequest;

class UnloadAssetBundleRequest
{
    public string abName;
    public bool unloadNow;
    public AssetBundleInfo abInfo;
}


public class AssetBundleManager : MonoSingleton<AssetBundleManager>
{
    AssetBundleManifest assetBundleManifest = null;
    Dictionary<string, AssetBundleInfo> loadedAssetBundles = new Dictionary<string, AssetBundleInfo>();    
    Dictionary<string, int> assetBundleLoading = new Dictionary<string, int>();
    Dictionary<string, string[]> dependencies = new Dictionary<string, string[]>();
    Dictionary<string, UnloadAssetBundleRequest> assetBundleUnloading = new Dictionary<string, UnloadAssetBundleRequest>();
    Dictionary<string, List<LoadUObjectAsyncRequest>> uobjectAsyncList = new Dictionary<string, List<LoadUObjectAsyncRequest>>();

    public override void Init()
    {
        if (ResConst.IsABMode)
        { 
            assetBundleManifest = ResManager.Instance.LoadAsset("Common", ResConst.RootFolderName.ToLower(), typeof(AssetBundleManifest)) as AssetBundleManifest;
        }
    }

    #region 同步加载
    public UObject LoadAssetBundleUObject(string abName, string assetName, Type type)
    {
        AssetBundleInfo bundle = GetLoadedAssetBundle(abName);
        if (bundle == null)
        {
            OnLoadAssetBundle(abName);
            bundle = GetLoadedAssetBundle(abName);
            if (bundle == null)
            {
                Debug.LogError("OnLoadAsset--->>>" + abName + " " + assetName);
            }
        }

        AssetBundle ab = bundle.assetBundle;
        var request = ab.LoadAsset(assetName, type);
        return request;
    }

    public AssetBundleInfo GetLoadedAssetBundle(string abName)
    {
        AssetBundleInfo bundle = null;
        loadedAssetBundles.TryGetValue(abName, out bundle);
        if (bundle == null)
        {
            return null;
        }
        string[] m_dependencies = null;
        if (!dependencies.TryGetValue(abName, out m_dependencies))
            return bundle;

        // Make sure all dependencies are loaded
        foreach (var dependency in m_dependencies)
        {
            AssetBundleInfo dependentBundle;
            loadedAssetBundles.TryGetValue(dependency, out dependentBundle);
            if (dependentBundle == null) return null;
        }
        return bundle;
    }

    public void OnLoadAssetBundle(string abName)
    {
        string path = ResPath.GetAssetBundleFilePath(abName);
        if (assetBundleLoading.ContainsKey(path))
        {
            assetBundleLoading[path]++;
            return;
        }
        assetBundleLoading.Add(path, 1);

        if (assetBundleManifest != null)
        {
            string[] dep = assetBundleManifest.GetAllDependencies(abName);
            if (dep.Length > 0)
            {
                if (!dependencies.ContainsKey(abName))
                    dependencies.Add(abName, dep);
                for (int i = 0; i < dep.Length; i++)
                {
                    string depName = dep[i];
                    AssetBundleInfo bundleInfo = null;
                    if (loadedAssetBundles.TryGetValue(depName, out bundleInfo))
                    {
                        bundleInfo.referencedCount++;
                    }
                    else
                    {
                        OnLoadAssetBundle(depName);
                    }
                }
            }
        }
        var assetObj = AssetBundle.LoadFromFile(path);
        if (assetObj != null)
        {
            //var RefCount = assetBundleLoading[path];
            var bundleInfo = new AssetBundleInfo(assetObj, 0);
            loadedAssetBundles.Add(abName, bundleInfo);
        }
    }

    #endregion

    #region 异步加载

    IEnumerator OnLoadAssetBundleAsync(string abName, bool isDep)
    {
        string path = ResPath.GetAssetBundleFilePath(abName);
        if (assetBundleLoading.ContainsKey(path) && isDep)
        {
            assetBundleLoading[path]++;
            yield break;
        }
        assetBundleLoading.Add(path, 1);
        var request = AssetBundle.LoadFromFileAsync(path);

        string[] dep = assetBundleManifest.GetAllDependencies(abName);
        if (dep.Length > 0)
        {
            if (!dependencies.ContainsKey(abName))
                dependencies.Add(abName, dep);
            for (int i = 0; i < dep.Length; i++)
            {
                string depName = dep[i];
                AssetBundleInfo bundleInfo = null;
                if (loadedAssetBundles.TryGetValue(depName, out bundleInfo))
                {
                    bundleInfo.referencedCount++;
                }
                else
                {
                    yield return StartCoroutine(OnLoadAssetBundleAsync(depName, true));
                }
            }
        }

        yield return request;

        AssetBundle assetObj = request.assetBundle;
        if (assetObj != null)
        {
            var refCount = assetBundleLoading[path];
            var bundleInfo = new AssetBundleInfo(assetObj, refCount);
            loadedAssetBundles.Add(abName, bundleInfo);
        }
        assetBundleLoading.Remove(path);
    }

    public void LoadAssetBundleUObjectAsync(string abName, string assetName, Type type, Action<UObject> sharpFunc = null)
    {
        LoadUObjectAsyncRequest request = new LoadUObjectAsyncRequest();
        request.assetNames = assetName;
        request.sharpFunc = sharpFunc;
        List<LoadUObjectAsyncRequest> requests = null;
        if (!uobjectAsyncList.TryGetValue(abName, out requests))
        {
            requests = new List<LoadUObjectAsyncRequest>();
            requests.Add(request);
            uobjectAsyncList.Add(abName, requests);
            StartCoroutine(OnLoadAssetAsync(abName, type));
        }
        else
        {
            requests.Add(request);
        }
    }

    IEnumerator OnLoadAssetAsync(string abName, Type type)
    {

        AssetBundleInfo bundleInfo = GetLoadedAssetBundle(abName);
        if (bundleInfo == null)
        {
            yield return StartCoroutine(OnLoadAssetBundleAsync(abName, false));
            bundleInfo = GetLoadedAssetBundle(abName);
            if (bundleInfo == null)
            {
                uobjectAsyncList.Remove(abName);
                Debug.LogError("OnLoadAsset--->>>" + abName);
                yield break;
            }
        }

        List<LoadUObjectAsyncRequest> requests = null;
        if (!uobjectAsyncList.TryGetValue(abName, out requests))
        {
            uobjectAsyncList.Remove(abName);
            yield break;
        }

        for (int i = 0; i < requests.Count; i++)
        {
            string assetNames = requests[i].assetNames;
            UObject result = new UObject();
            
            AssetBundle ab = bundleInfo.assetBundle;
            if (!ab.isStreamedSceneAssetBundle)
            {
                var request = ab.LoadAssetAsync(assetNames, type);
                yield return request;
                result = request.asset;
            }
           
            if (requests[i].sharpFunc != null)
            {
                requests[i].sharpFunc(result);
                requests[i].sharpFunc = null;
            }
            
        }
        bundleInfo.referencedCount = requests.Count;
        uobjectAsyncList.Remove(abName);        
    }
    #endregion

    #region 资源卸载
    /// <summary>
    /// 试着去卸载AB
    /// </summary>
    public void UnloadAssetBundle(string abName, bool isThorough = false)
    {
        uobjectAsyncList.Remove(abName);
        UnloadAssetBundleInternal(abName, isThorough);
        UnloadDependencies(abName, isThorough);
    }


    private void Update()
    {
        TryUnloadAssetBundle();
    }

    private void TryUnloadAssetBundle()
    {
        if (assetBundleUnloading.Count == 0)
        {
            return;
        }
        foreach (var de in assetBundleUnloading)
        {
            if (assetBundleLoading.ContainsKey(de.Key))
            {
                continue;
            }
            var request = de.Value;

            if (request.abInfo != null && request.abInfo.assetBundle != null)
            {
                request.abInfo.assetBundle.Unload(true);
            }
            assetBundleUnloading.Remove(de.Key);
            loadedAssetBundles.Remove(de.Key);
            Debug.Log(de.Key + " has been unloaded successfully");
        }
    }


    private void UnloadAssetBundleInternal(string abName, bool unloadNow)
    {
        AssetBundleInfo bundle = GetLoadedAssetBundle(abName);
        if (bundle == null) return;

        if (--bundle.referencedCount <= 0)
        {
            if (assetBundleLoading.ContainsKey(abName))
            {
                var request = new UnloadAssetBundleRequest();
                request.abName = abName;
                request.abInfo = bundle;
                request.unloadNow = unloadNow;
                assetBundleUnloading.Add(abName, request);
                return;     //如果当前AB处于Async Loading过程中，卸载会崩溃，只减去引用计数即可
            }
            bundle.assetBundle.Unload(unloadNow);
            loadedAssetBundles.Remove(abName);
            Debug.Log(abName + " has been unloaded successfully");
        }
    }

    private void UnloadDependencies(string abName, bool isThorough)
    {
        string[] dep = null;
        if (!dependencies.TryGetValue(abName, out dep))
            return;

        // Loop dependencies.
        foreach (var value in dep)
        {
            UnloadAssetBundleInternal(value, isThorough);
        }
        dependencies.Remove(abName);
    }
    #endregion

    private void OnDestroy()
    {
        assetBundleManifest = null;
        loadedAssetBundles.Clear();
        assetBundleLoading.Clear();
        dependencies.Clear();
        assetBundleUnloading.Clear();
        uobjectAsyncList.Clear();
    }
}


public class AssetBundleInfo
{
    public AssetBundle assetBundle;
    public int referencedCount;

    public AssetBundleInfo(AssetBundle ab, int RefCount = 0)
    {
        assetBundle = ab;
        referencedCount = RefCount;
    }
}


public class LoadUObjectAsyncRequest
{
    public UObject uObject;
    public string assetNames;
    public Action<UObject> sharpFunc;
    public Type assetType;
}
