using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

internal static class MonoSingletonRoot
{
    public static bool IsQuitting;
    public static GameObject Go;

    public static GameObject GetOrCreate()
    {
        if (Go == null)
        {
            Go = new GameObject("Singleton");
            Object.DontDestroyOnLoad(Go);
        }
        return Go;
    }

    public static void DestroyRoot()
    {
        if (Go != null)
        {
            if (Application.isPlaying)
                Object.Destroy(Go);
            else
                Object.DestroyImmediate(Go);
        }
        Go = null;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics()
    {
        Go = null;
        IsQuitting = false;
    }
}

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
                if (MonoSingletonRoot.IsQuitting || !Application.isPlaying)
                    return null;
                isInit = true;

                GameObject go = MonoSingletonRoot.GetOrCreate();
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
        MonoSingletonRoot.IsQuitting = true;
        m_instance = null;
        isInit = true;
    }
}

#if UNITY_EDITOR
[InitializeOnLoad]
static class MonoSingletonEditorCleanup
{
    static MonoSingletonEditorCleanup()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            MonoSingletonRoot.IsQuitting = false;
            MonoSingletonRoot.Go = null;
            return;
        }

        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            MonoSingletonRoot.IsQuitting = true;
            return;
        }

        if (state != PlayModeStateChange.EnteredEditMode)
            return;

        MonoSingletonRoot.DestroyRoot();
        var leftover = GameObject.Find("Singleton");
        if (leftover != null)
            Object.DestroyImmediate(leftover);
    }
}
#endif
