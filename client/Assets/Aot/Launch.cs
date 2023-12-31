using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Launch : MonoBehaviour
{

    async void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        App.AppConfig = Resources.Load<AppConfig>("AppConfig");
        Debug.unityLogger.logEnabled = App.AppConfig.DebugLog;
        QualitySettings.vSyncCount = 2;
        Application.targetFrameRate = App.AppConfig.GameFrameRate;

        await AotRes.Instance.InitUniTask();
        await AotText.Instance.InitUniTask();
        //await AotUpdate.Instance.CheckExtractStreamingAssets();
        await AotRes.Instance.LoadAssetAsync<TMP_FontAsset>("Assets/App/Font/SourceHanSansCN-NormalSDF.asset");
        await AotRes.Instance.LoadAssetAsync<TMP_FontAsset>("Assets/App/Font/SourceHanSerifCN-BoldSDF.asset");
        AotUI.Instance.Open("SplashAdvicePanel");
     
    }
}
