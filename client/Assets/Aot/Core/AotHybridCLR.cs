using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HybridCLR;
using System.Reflection;
using System;
using System.Linq;

public class AotHybridCLR : AotSingleton<AotHybridCLR>
{
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
        TextAsset ta = AotRes.Instance.LoadAsset<TextAsset>("app/dll/dll.unity3d", "Assets/App/Dll/Hotfix.dll.bytes");
        _hotUpdateAss = Assembly.Load(ta.bytes);
#else
        _hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Hotfix");
#endif
        AotRes.Instance.UnLoadAssetBundle();
        Debug.Log("dll�������");
        Type entryType = _hotUpdateAss.GetType("Launch");
        entryType.GetMethod("Start").Invoke(null, null);
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
            TextAsset ta = AotRes.Instance.LoadAsset<TextAsset>("app/dll/dll.unity3d", "Assets/App/Dll/" + aotDllName);
            byte[] dllBytes = ta.bytes;
            // ����assembly��Ӧ��dll�����Զ�Ϊ��hook��һ��aot���ͺ�����native���������ڣ��ý������汾����
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
        }
    }
}
