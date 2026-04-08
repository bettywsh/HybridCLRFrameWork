using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidClass {

    private static AndroidJavaObject m_unityActivity;
    public static AndroidJavaObject UnityJavaObject
    {
        get
        {
            if (m_unityActivity == null)
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
                m_unityActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return m_unityActivity;
        }
    }
}
