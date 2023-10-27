using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Launch
{
    // Start is called before the first frame update
    public static async void Start()
    {
        AotUI.Instance.Close("UpdatePanel");
        AotText.Instance.Dispose();

        Debug.unityLogger.logEnabled = App.AppConfig.DebugLog;
        QualitySettings.vSyncCount = 2;
        Application.targetFrameRate = App.AppConfig.GameFrameRate;

        SoundManager.Instance.Init();
        UIManager.Instance.Init();
        AtlasManager.Instance.Init();
        ResManager.Instance.Init();
        NetworkManager.Instance.Init();
        ConfigManager.Instance.Init();
        await ResManager.Instance.CommonLoadAssetAsync<TMP_FontAsset>("Assets/App/Font/SourceHanSansCN-NormalSDF.asset");
        await ResManager.Instance.CommonLoadAssetAsync<TMP_FontAsset>("Assets/App/Font/SourceHanSerifCN-BoldSDF.asset");
        LoadSceneManager.Instance.LoadScene("Login", false);
        //UIManager.Instance.Open("LoginPanel");
    }

 
}
