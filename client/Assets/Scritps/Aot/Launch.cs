using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Launch : MonoBehaviour
{

    async void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        AppSettings.AppConfig = Resources.Load<AppConfig>("AppConfig");
        Debug.unityLogger.logEnabled = AppSettings.AppConfig.DebugLog;
        QualitySettings.vSyncCount = 2;
        Application.targetFrameRate = AppSettings.AppConfig.GameFrameRate;

        await ResManager.Instance.InitUniTask();
        await AotText.Instance.InitUniTask();
        //await AotUpdate.Instance.CheckExtractStreamingAssets();
        await ResManager.Instance.SceneLoadAssetAsync<TMP_FontAsset>("Assets/App/Font/SourceHanSansCN-NormalSDF.asset");
        await ResManager.Instance.SceneLoadAssetAsync<TMP_FontAsset>("Assets/App/Font/SourceHanSerifCN-BoldSDF.asset");
        UIManager.Instance.Open<SplashAdvicePanel>();
     
    }
}
