using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HybridCLR;
using System.Reflection;
using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class HybridCLRManager : AotSingleton<HybridCLRManager>
{
    private List<string> AOTMetaAssemblyFiles { get; } = new List<string>()
    {
        "mscorlib.dll.bytes",
        "System.dll.bytes",
        "System.Core.dll.bytes",
    };
    public Assembly _hotUpdateAss;
    public async UniTask LoadDll()
    {
        await LoadMetadataForAOTAssemblies();
#if !UNITY_EDITOR
        TextAsset ta = await ResManager.Instance.SceneLoadAssetAsync<TextAsset>("Assets/App/Dll/Hotfix.dll.bytes");
        _hotUpdateAss = Assembly.Load(ta.bytes);
#else
        _hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Hotfix");
#endif
        Debug.Log("dll加载完成");
        Type entryType = _hotUpdateAss.GetType("Launch");
        
        entryType.GetMethod("Start").Invoke(null, null);
    }

    /// <summary>
    /// 为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
    /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
    /// </summary>
    private async UniTask LoadMetadataForAOTAssemblies()
    {
        /// 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
        /// 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
        /// 
        HomologousImageMode mode = HomologousImageMode.SuperSet;
        foreach (var aotDllName in AOTMetaAssemblyFiles)
        {
            TextAsset ta = await AotResManager.Instance.LoadAsset<TextAsset>($"Assets/App/Dll/{aotDllName}");
            // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(ta.bytes, mode);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
        }
    }

}
