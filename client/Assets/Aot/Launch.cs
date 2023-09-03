using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Launch : MonoBehaviour
{

    void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        TMP_FontAsset a = AotRes.Instance.LoadAsset<TMP_FontAsset>("App/Font/SourceHanSansCN-Normal SDF.unity3d", "Assets/App/Font/SourceHanSansCN-Normal SDF.asset");
        TMP_FontAsset b = AotRes.Instance.LoadAsset<TMP_FontAsset>("App/Font/SourceHanSerifCN-Bold SDF.unity3d", "Assets/App/Font/SourceHanSerifCN-Bold SDF.asset");
        AotUI.Instance.Open("SplashAdvicePanel");
        
    }
}
