using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public abstract class Singleton<T> : IDisposable where T : class, new()
{
	private static T m_instance;
	public static T Instance
	{
		get
        {
            if (m_instance == null)
		    {
			    m_instance = Activator.CreateInstance<T>();
				//if (m_instance != null)
				//{
				//	(m_instance as Singleton<T>).Init();
				//}
			}

            return m_instance;
        }
	}

    public virtual async UniTask Init()
    {
		await UniTask.Yield();
    }


    public virtual void Dispose()
	{
        m_instance = null;
	}

}
