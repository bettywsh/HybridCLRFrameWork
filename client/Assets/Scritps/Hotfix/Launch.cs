using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Launch
{
    // Start is called before the first frame update
    public static async void Start()
    {
        UIManager.Instance.Close<UpdatePanel>();
        AotText.Instance.Dispose();
        ResManager.Instance.UnLoadAssetBundle(LoadSceneManager.Instance.CurScene());
        LoadSceneManager.Instance.Dispose();
        ResManager.Instance.Dispose();
        AotDialog.Instance.Dispose();

        //await ResManager.Instance.Init();
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
