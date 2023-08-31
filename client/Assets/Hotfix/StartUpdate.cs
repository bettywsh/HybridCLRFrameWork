using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        UIManager.Instance.Open("SplashAdvicePanel");
    }

 
}
