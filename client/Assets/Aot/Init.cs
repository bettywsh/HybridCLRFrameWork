using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Reflection;
using System;
using System.Linq;
using HybridCLR;

public class Init :MonoBehaviour
{
    Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();

    void Awake()
    {
        LoadDll();
    }

    public T LoadAssetBundle<T>(string abName, string assetName) where  T : UnityEngine.Object 
    {
        string path = GetPersistentPath() + abName;
        if (!File.Exists(path))
        {
            path = GetStreamingAssetsPath() + abName;
        }
        AssetBundle ab;
        if (!abDic.TryGetValue(abName, out ab))
        {
            ab = AssetBundle.LoadFromFile(path);
            abDic.Add(abName, ab);
        }     
        return ab.LoadAsset<T>(assetName);
    }

    public void UnLoadAssetBundle()
    {
         List<KeyValuePair<string, UnityEngine.AssetBundle>> abs = abDic.ToList();
        for (int i = abs.Count - 1; i >= 0; i--)
        {
            abs[i].Value.Unload(true);            
        }
        abDic.Clear();
        abDic = null;
    }

    string GetPersistentPath()
    {
        return Application.persistentDataPath + "/";
    }

    string GetStreamingAssetsPath()
    {
#if UNITY_EDITOR
        return Application.streamingAssetsPath + "/";
#else
#if UNITY_ANDROID
        return Application.dataPath + @"/Raw" + "/";
#elif UNITY_IOS
        return Application.streamingAssetsPath + "/";
#else
        return Application.streamingAssetsPath + "/";
#endif
#endif
    }


    private List<string> AOTMetaAssemblyFiles { get; } = new List<string>()
    {
        "mscorlib.dll.bytes",
        "System.dll.bytes",
        "System.Core.dll.bytes",
    };
    private Assembly _hotUpdateAss;
    public void LoadDll()
    {
        LoadMetadataForAOTAssemblies();
#if !UNITY_EDITOR
        TextAsset ta = LoadAssetBundle<TextAsset>("app/dll/dll.unity3d", "Assets/App/Dll/Hotfix.dll.bytes");
        _hotUpdateAss = Assembly.Load(ta.bytes);
#else
        _hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Hotfix");
#endif
        UnLoadAssetBundle();
        Type entryType = _hotUpdateAss.GetType("StartUpdate");
        entryType.GetMethod("Start").Invoke(null, null);

        Debug.Log("dll加载完成");
    }

    /// <summary>
    /// 为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
    /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
    /// </summary>
    private void LoadMetadataForAOTAssemblies()
    {
        /// 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
        /// 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
        /// 
        HomologousImageMode mode = HomologousImageMode.SuperSet;
        foreach (var aotDllName in AOTMetaAssemblyFiles)
        {
            TextAsset ta = LoadAssetBundle<TextAsset>("app/dll/dll.unity3d", "Assets/App/Dll/" + aotDllName);
            byte[] dllBytes = ta.bytes;
            // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
        }
    }
}
