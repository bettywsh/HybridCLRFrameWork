using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using YooAsset;
using System.Reflection;
using Game;
using UnityEngine.U2D;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SimpleJSON;

public class Launch
{
    public static async void Start()
    {
        SDKManager.Instance.KuaiShouChannel();
        var updatePanel = AotUIManager.Instance.GetUI<UpdatePanel>();
        updatePanel?.SetTitle("加载游戏资源");
        updatePanel?.SetProgressTween(true);
        await UniTask.Yield();
       
        //初始化hotfix管理器
        AssemblyManager.Instance.Init(new Assembly[1]{ HybridCLRManager.Instance._hotUpdateAss });

        await ResManager.Instance.Init();
        await SoundManager.Instance.Init();
        await UIManager.Instance.Init();
        await AtlasManager.Instance.Init();
        await ConfigManager.Instance.Init();
        await TimeManager.Instance.Init();
        await TimerManager.Instance.Init();
        await DialogManager.Instance.Init();
        await DataManager.Instance.Init();
        NetworkManager.Instance.Init(() => {
            DialogManager.Instance.ShowNetLoading();
        }, () => {
            DialogManager.Instance.HideNetLoading();
        });
        await RedManager.Instance.Init();

        //通用界面预加载

        //战斗资源

        //await CardBuffSetting.The.Init();
        await LoadSceneManager.Instance.Init(MessageConst.Msg_LoadingPanelProgress, MessageConst.Msg_LoadingPanelComplete);

        updatePanel?.SetProgressTween(false);
        await UniTask.Yield();
        UIManager.Instance.Open<LoginPanel>();
    }

}
