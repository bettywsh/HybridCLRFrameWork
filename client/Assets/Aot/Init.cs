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

        Debug.Log("dll�������");
    }

    /// <summary>
    /// Ϊaot assembly����ԭʼmetadata�� ��������aot�����ȸ��¶��С�
    /// һ�����غ����AOT���ͺ�����Ӧnativeʵ�ֲ����ڣ����Զ��滻Ϊ����ģʽִ��
    /// </summary>
    private void LoadMetadataForAOTAssemblies()
    {
        /// ע�⣬����Ԫ�����Ǹ�AOT dll����Ԫ���ݣ������Ǹ��ȸ���dll����Ԫ���ݡ�
        /// �ȸ���dll��ȱԪ���ݣ�����Ҫ���䣬�������LoadMetadataForAOTAssembly�᷵�ش���
        /// 
        HomologousImageMode mode = HomologousImageMode.SuperSet;
        foreach (var aotDllName in AOTMetaAssemblyFiles)
        {
            TextAsset ta = LoadAssetBundle<TextAsset>("app/dll/dll.unity3d", "Assets/App/Dll/" + aotDllName);
            byte[] dllBytes = ta.bytes;
            // ����assembly��Ӧ��dll�����Զ�Ϊ��hook��һ��aot���ͺ�����native���������ڣ��ý������汾����
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
        }
    }
}
