using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartUpdate 
{
    // Start is called before the first frame update
    public static void Start()
    {
        Debug.unityLogger.logEnabled = AppConst.DebugLog;
        QualitySettings.vSyncCount = 2;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = AppConst.GameFrameRate;

        SoundManager.Instance.Init();
        UIManager.Instance.Init();
        AtlasManager.Instance.Init();
        ResManager.Instance.Init();
        AssetBundleManager.Instance.Init();
        //ResManager.Instance.LoadAssetAsync("Common", "Font/SourceHanSansCN-Normal SDF.asset", typeof(TMP_FontAsset));
        //ResManager.Instance.LoadAssetAsync("Common", "Font/SourceHanSerifCN-Bold SDF.asset", typeof(TMP_FontAsset));
        UIManager.Instance.Open("SplashAdvicePanel");
    }

 
}
