using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Launch : MonoBehaviour
{

    async void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        await AotUpdate.Instance.CheckExtractStreamingAssets();
        AotRes.Instance.LoadAsset<TMP_FontAsset>("App/Font/SourceHanSansCN-NormalSDF.unity3d", "Assets/App/Font/SourceHanSansCN-NormalSDF.asset");
        AotRes.Instance.LoadAsset<TMP_FontAsset>("App/Font/SourceHanSerifCN-BoldSDF.unity3d", "Assets/App/Font/SourceHanSerifCN-BoldSDF.asset");
        AotUI.Instance.Open("SplashAdvicePanel");
        AotText.Instance.Init();
    }
}
