using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Launch : MonoBehaviour
{

    async void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        AppSettings.AppConfig = Resources.Load<AppConfig>("AppConfig");
        GameObject.Find("Reporter").SetActive(AppSettings.AppConfig.DebugLog);
        Debug.unityLogger.logEnabled = AppSettings.AppConfig.DebugLog;
        QualitySettings.vSyncCount = 2;
        Application.targetFrameRate = AppSettings.AppConfig.GameFrameRate;


        await AotResManager.Instance.Init();
        //await AotText.Instance.Init();
        await AotResManager.Instance.LoadAsset<TMP_FontAsset>("Assets/App/Font/SourceHanSansCN-NormalSDF.asset");
        await AotResManager.Instance.LoadAsset<TMP_FontAsset>("Assets/App/Font/SourceHanSerifCN-BoldSDF.asset");
        await AotUIManager.Instance.Init();
        AotUIManager.Instance.Open<SplashAdvicePanel>();
     
    }
}
