using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T>: MonoBehaviour where T : MonoBehaviour
{
    protected static bool isInit = false;
    protected static T m_instance = null;

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                if (SingletonData.IsQuitting || !Application.isPlaying)
                    return null;
                isInit = true;

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

    public virtual void Dispose()
    {
        m_instance = null;
        isInit = false;
    }

    public virtual void OnDestroy()
    {
        Dispose();
    }

    void OnApplicationQuit()
    {
        SingletonData.IsQuitting = true;
        m_instance = null;
        isInit = true;
    }
}
