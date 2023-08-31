using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HybridCLR;
using System.Linq;
using System;
using System.Reflection;

public class HybridClrManager : Singleton<HybridClrManager>
{
    private static List<string> AOTMetaAssemblyFiles { get; } = new List<string>()
    {
        "mscorlib.dll.bytes",
        "System.dll.bytes",
        "System.Core.dll.bytes",
    };
    private static Assembly _hotUpdateAss;
    public void LoadDll()
    {
        LoadMetadataForAOTAssemblies();
#if !UNITY_EDITOR
        TextAsset ta = ResManager.Instance.LoadAsset("dll", "HotUpdate.dll", typeof(TextAsset)) as TextAsset;
        _hotUpdateAss = Assembly.Load(ta.bytes);
#else
        _hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "HotUpdate");
#endif
        //Type entryType = _hotUpdateAss.GetType("Entry");
        //entryType.GetMethod("Start").Invoke(null, null);

        Debug.Log("dll加载完成");
    }

    /// <summary>
    /// 为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
    /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
    /// </summary>
    private static void LoadMetadataForAOTAssemblies()
    {
        /// 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
        /// 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
        /// 
        HomologousImageMode mode = HomologousImageMode.SuperSet;
        foreach (var aotDllName in AOTMetaAssemblyFiles)
        {
            TextAsset ta = ResManager.Instance.LoadAsset("dll", aotDllName, typeof(TextAsset)) as TextAsset;
            byte[] dllBytes = ta.bytes;
            // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
        }
    }
}
