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
        ResManager.Instance.Dispose();
        AotDialog.Instance.Dispose();
        AotUpdate.Instance.Dispose();

        SoundManager.Instance.Init();
        UIManager.Instance.Init();
        AtlasManager.Instance.Init();
        ResManager.Instance.Init();
        NetworkManager.Instance.Init();
        ConfigManager.Instance.Init();
        await ResManager.Instance.CommonLoadAssetAsync<TMP_FontAsset>("Assets/App/Font/SourceHanSansCN-NormalSDF.asset");
        await ResManager.Instance.CommonLoadAssetAsync<TMP_FontAsset>("Assets/App/Font/SourceHanSerifCN-BoldSDF.asset");
        LoadSceneManager.Instance.Init(()=> UIManager.Instance.Open<LoadingPanel>());
        LoadSceneManager.Instance.LoadScene((int)EScene.Login, false);
        //UIManager.Instance.Open("LoginPanel");
    }

 
}
