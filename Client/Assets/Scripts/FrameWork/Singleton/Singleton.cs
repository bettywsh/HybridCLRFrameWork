using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public abstract class Singleton<T> : SingletonData, IDisposable where T : class, new()
{
    private static T m_instance = null;
    private static int s_playSession;

    public static T Instance
    {
        get
        {
            if (s_playSession != PlaySession)
            {
                m_instance = null;
                s_playSession = PlaySession;
            }

            if (m_instance == null)
            {
                if (IsQuitting || !Application.isPlaying)
                    return null;
                m_instance = Activator.CreateInstance<T>();
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
    }
}
