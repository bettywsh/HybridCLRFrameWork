using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T>: MonoBehaviour where T : MonoBehaviour
{
    protected static T m_instance = null;
    private static int s_playSession;

    public static T Instance
    {
        get
        {
            if (s_playSession != SingletonData.PlaySession)
            {
                m_instance = null;
                s_playSession = SingletonData.PlaySession;
            }

            if (m_instance == null)
            {
                if (SingletonData.IsQuitting || !Application.isPlaying)
                    return null;

                GameObject go = GameObject.Find("Singleton");
                if (go != null && go.scene.name != "DontDestroyOnLoad")
                    go = null;
                if (go == null)
                {
                    go = new GameObject("Singleton");
                    DontDestroyOnLoad(go);
                }
                Transform trans = go.transform.Find(typeof(T).Name);
                if (trans == null)
                {
                    trans = new GameObject(typeof(T).Name).transform;
                    trans.SetParent(go.transform, false);
                }
                m_instance = trans.GetComponent<T>();
                if (m_instance == null)
                {
                    m_instance = trans.gameObject.AddComponent<T>();
                }
            }
            return m_instance;
        }
    }

    public virtual void Init()
    {

    }

    public virtual void OnDestroy()
    {
         m_instance = null;
    }
}
