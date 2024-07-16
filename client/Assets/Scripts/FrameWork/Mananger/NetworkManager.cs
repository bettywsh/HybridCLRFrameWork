using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum EServer { 
	Login = 1
}

public class NetworkManager : MonoSingleton<NetworkManager>
{
	public Dictionary<NetworkProtocol, Session> Sessions = new Dictionary<NetworkProtocol, Session>();

	public override async UniTask Init()
	{
		await base.Init();
    }

	public Session Create(NetworkProtocol networkProtocol)
	{
        Session session = GetSession(networkProtocol);
		if (session == null)
		{
			session = new Session((int)networkProtocol, networkProtocol);
            Sessions.Add(networkProtocol, session);
        }
		return session;
    }

    public void Update()
    {
        foreach (var session in Sessions)
		{
			session.Value.Update();
		}
    }

    public Session GetSession(NetworkProtocol networkProtocol)
	{
		Session session;
        if (!Sessions.TryGetValue(networkProtocol, out session))
		{
			return null;
		}
		return session;

    }
	
}