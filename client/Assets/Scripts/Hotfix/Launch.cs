using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using YooAsset;

public class Launch
{
    // Start is called before the first frame update
    public static async void Start()
    {
        AotDialogManager.Instance.Dispose();
        AotHttpManager.Instance.Dispose();
        AotUIManager.Instance.Dispose();
        YooAssets.DestroyPackage(AppSettings.AppConfig.PackageName);
        YooAssets.Destroy();
        AotResManager.Instance.UnLoadAssetBundle(SceneManager.GetActiveScene().name);
        AotResManager.Instance.Dispose();

        await ResManager.Instance.Init();
        await SoundManager.Instance.Init();
        await UIManager.Instance.Init();
        await AtlasManager.Instance.Init();
        await NetworkManager.Instance.Init();
        await ConfigManager.Instance.Init();
        await ResManager.Instance.CommonLoadAssetAsync<TMP_FontAsset>("Assets/App/Font/SourceHanSansCN-NormalSDF.asset");
        await ResManager.Instance.CommonLoadAssetAsync<TMP_FontAsset>("Assets/App/Font/SourceHanSerifCN-BoldSDF.asset");
        LoadSceneManager.Instance.Init(()=> UIManager.Instance.Open<LoadingPanel>());
        LoadSceneManager.Instance.LoadScene(EScene.Login.ToString(), false);
        //UIManager.Instance.Open("LoginPanel");
    }

 
}
