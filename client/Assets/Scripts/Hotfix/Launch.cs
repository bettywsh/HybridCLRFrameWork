using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using YooAsset;
using System.Reflection;

public class Launch
{
    public static async void Start()
    {
        //销毁aot管理器
        AotDialogManager.Instance.Dispose();
        AotHttpManager.Instance.Dispose();
        AotUIManager.Instance.Dispose();
        AotResManager.Instance.Dispose();

        //初始化hotfix管理器
        AssemblyManager.Instance.Init(new Assembly[1]{ HybridCLRManager.Instance._hotUpdateAss });
        await ResManager.Instance.Init();
        await SoundManager.Instance.Init();
        await UIManager.Instance.Init();
        await AtlasManager.Instance.Init();
        await NetworkManager.Instance.Init();
        await ConfigManager.Instance.Init();
        await TimerManager.Instance.Init();
        await DialogManager.Instance.Init();
        await ResManager.Instance.CommonLoadAssetAsync<TMP_FontAsset>("Assets/App/Font/SourceHanSansCN-NormalSDF.asset");
        await ResManager.Instance.CommonLoadAssetAsync<TMP_FontAsset>("Assets/App/Font/SourceHanSerifCN-BoldSDF.asset");
        await LoadSceneManager.Instance.Init(MessageConst.Msg_LoadingPanelProgress, MessageConst.Msg_LoadingPanelComplete);
        LoadSceneManager.Instance.LoadScene(EScene.Login.ToString(), false);
    }

 
}
