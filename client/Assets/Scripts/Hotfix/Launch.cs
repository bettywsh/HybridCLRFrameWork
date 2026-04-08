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
        //await ResManager.Instance.CommonLoadAssetAsync<TMP_FontAsset>("Assets/App/Font/PuHuiTi SDF.asset");
        await ResManager.Instance.CommonLoadAssetAsync<SpriteAtlas>("Assets/App/Atlas/Common.spriteatlasv2");
        await ResManager.Instance.CommonLoadAssetAsync<SpriteAtlas>("Assets/App/Atlas/Common1.spriteatlasv2");
        await ResManager.Instance.CommonLoadAssetAsync<SpriteAtlas>($"Assets/App/Atlas/LoadingPanel.spriteatlasv2");
        await ResManager.Instance.CommonLoadAssetAsync<SpriteAtlas>($"Assets/App/Atlas/TextPanel.spriteatlasv2");
        await ResManager.Instance.CommonLoadAssetAsync<SpriteAtlas>($"Assets/App/Atlas/NetLoadingPanel.spriteatlasv2");
        await ResManager.Instance.CommonLoadAssetAsync<SpriteAtlas>($"Assets/App/Atlas/DialogPanel.spriteatlasv2");
        await ResManager.Instance.CommonLoadAssetAsync<SpriteAtlas>($"Assets/App/Atlas/GuidePanel.spriteatlasv2");
        //await ResManager.Instance.CommonLoadAssetAsync<SpriteAtlas>($"Assets/App/Atlas/EntrancePanel.spriteatlasv2");

        await ResManager.Instance.CommonLoadAssetAsync<GameObject>($"Assets/App/Prefab/UI/Panel/ReConnectLoadingPanel.prefab");
        await ResManager.Instance.CommonLoadAssetAsync<GameObject>($"Assets/App/Prefab/UI/Panel/LoadingPanel.prefab");
        await ResManager.Instance.CommonLoadAssetAsync<GameObject>($"Assets/App/Prefab/UI/Panel/BattleLoadingPanel.prefab");
        await ResManager.Instance.CommonLoadAssetAsync<GameObject>($"Assets/App/Prefab/UI/Panel/TextPanel.prefab");
        await ResManager.Instance.CommonLoadAssetAsync<GameObject>($"Assets/App/Prefab/UI/Panel/NetLoadingPanel.prefab");
        await ResManager.Instance.CommonLoadAssetAsync<GameObject>($"Assets/App/Prefab/UI/Panel/DialogPanel.prefab");
        await ResManager.Instance.CommonLoadAssetAsync<GameObject>("Assets/App/Prefab/UI/Panel/DialogSystemPanel.prefab");
        await ResManager.Instance.CommonLoadAssetAsync<GameObject>($"Assets/App/Prefab/UI/Panel/GuidePanel.prefab");
        //await ResManager.Instance.CommonLoadAssetAsync<GameObject>("Assets/App/Prefab/UI/Panel/EntrancePanel.prefab");
        //await ResManager.Instance.CommonLoadAssetAsync<GameObject>("Assets/App/Prefab/UI/SubPanel/HallSubPanel.prefab");

        //战斗资源

        //await CardBuffSetting.The.Init();
        await LoadSceneManager.Instance.Init(MessageConst.Msg_LoadingPanelProgress, MessageConst.Msg_LoadingPanelComplete);

        updatePanel?.SetProgressTween(false);
        await UniTask.Yield();

    }

    

    //模拟器判断
    private static bool IsRunningOnEmulator()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
	if(SystemInfo.graphicsDeviceID == 0 || SystemInfo.graphicsDeviceVendorID == 0)
	{	
        AndroidJavaClass buildClass = new AndroidJavaClass("android.os.Build");
		string radioVersion = buildClass.CallStatic<string>("getRadioVersion");
		if (radioVersion.Length > 0)
			return true;
	}
   
	return false;

#else
        return false; // 在其他平台或编辑器中默认不在模拟器中运行
#endif
    }
}
