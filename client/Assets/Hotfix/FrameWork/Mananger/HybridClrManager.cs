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

        Debug.Log("dll�������");
    }

    /// <summary>
    /// Ϊaot assembly����ԭʼmetadata�� ��������aot�����ȸ��¶��С�
    /// һ�����غ����AOT���ͺ�����Ӧnativeʵ�ֲ����ڣ����Զ��滻Ϊ����ģʽִ��
    /// </summary>
    private static void LoadMetadataForAOTAssemblies()
    {
        /// ע�⣬����Ԫ�����Ǹ�AOT dll����Ԫ���ݣ������Ǹ��ȸ���dll����Ԫ���ݡ�
        /// �ȸ���dll��ȱԪ���ݣ�����Ҫ���䣬�������LoadMetadataForAOTAssembly�᷵�ش���
        /// 
        HomologousImageMode mode = HomologousImageMode.SuperSet;
        foreach (var aotDllName in AOTMetaAssemblyFiles)
        {
            TextAsset ta = ResManager.Instance.LoadAsset("dll", aotDllName, typeof(TextAsset)) as TextAsset;
            byte[] dllBytes = ta.bytes;
            // ����assembly��Ӧ��dll�����Զ�Ϊ��hook��һ��aot���ͺ�����native���������ڣ��ý������汾����
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
        }
    }
}
