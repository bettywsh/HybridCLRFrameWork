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
        AssemblyManager.Instance.Init();

        ResManager.Instance.Init();
        SoundManager.Instance.Init();
        UIManager.Instance.Init();
        AtlasManager.Instance.Init();
        ConfigManager.Instance.Init();
        TimeManager.Instance.Init();
        TimerManager.Instance.Init();
        DialogManager.Instance.Init();
        DataManager.Instance.Init();
        NetworkManager.Instance.Init();
        RedManager.Instance.Init();

        //通用界面预加载

        //战斗资源

        //await CardBuffSetting.The.Init();
        LoadSceneManager.Instance.Init();

        updatePanel?.SetProgressTween(false);
        await UniTask.Yield();

        NetworkManager.Instance.SetNetLoading(() =>
        {
            DialogManager.Instance.ShowNetLoading();
        }, () =>
        {
            DialogManager.Instance.HideNetLoading();
        });
;
        




        UIManager.Instance.Open<LoginPanel>().Forget();
    }

}
