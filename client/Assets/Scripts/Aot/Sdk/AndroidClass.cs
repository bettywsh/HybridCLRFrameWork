using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidClass {


    private static AndroidJavaClass m_javaClass;
    public static AndroidJavaClass UnityJavaClass
    {
        get
        {
            if (m_javaClass == null)
            {
                m_javaClass = new AndroidJavaClass("com.djl28.fish3d.activity.MainActivity");
            }
            return m_javaClass;
        }
    }

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
