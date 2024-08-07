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
    public Assembly _hotUpdateAss;
    public Assembly _frameWorkAss;
    public async UniTask LoadDll()
    {
        await LoadMetadataForAOTAssemblies();
#if !UNITY_EDITOR
        TextAsset ta = await AotResManager.Instance.LoadAsset<TextAsset>("Assets/App/Dll/FrameWork.dll.bytes");
        _frameWorkAss = Assembly.Load(ta.bytes);
        ta = await AotResManager.Instance.LoadAsset<TextAsset>("Assets/App/Dll/Hotfix.dll.bytes");
        _hotUpdateAss = Assembly.Load(ta.bytes);
#else
        _frameWorkAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "FrameWork");
        _hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Hotfix");
#endif
        Debug.Log("dll�������");
        Type entryType = _hotUpdateAss.GetType("Launch");
        
        entryType.GetMethod("Start").Invoke(null, null);
    }

    /// <summary>
    /// Ϊaot assembly����ԭʼmetadata�� ��������aot�����ȸ��¶��С�
    /// һ�����غ����AOT���ͺ�����Ӧnativeʵ�ֲ����ڣ����Զ��滻Ϊ����ģʽִ��
    /// </summary>
    private async UniTask LoadMetadataForAOTAssemblies()
    {
        /// ע�⣬����Ԫ�����Ǹ�AOT dll����Ԫ���ݣ������Ǹ��ȸ���dll����Ԫ���ݡ�
        /// �ȸ���dll��ȱԪ���ݣ�����Ҫ���䣬�������LoadMetadataForAOTAssembly�᷵�ش���
        /// 
        HomologousImageMode mode = HomologousImageMode.SuperSet;
        List<string> AOTMetaAssemblyFiles = AppSettings.AppConfig.AotDll;
        foreach (var aotDllName in AOTMetaAssemblyFiles)
        {
            TextAsset ta = await AotResManager.Instance.LoadAsset<TextAsset>($"Assets/App/Dll/{aotDllName}.bytes");
            // ����assembly��Ӧ��dll�����Զ�Ϊ��hook��һ��aot���ͺ�����native���������ڣ��ý������汾����
            LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(ta.bytes, mode);
            Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
        }
    }

}
