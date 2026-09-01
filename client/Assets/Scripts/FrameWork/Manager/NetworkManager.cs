using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public enum EServer { 
	Login = 1
}

public class NetworkManager : MonoSingleton<NetworkManager>
{
	public Dictionary<NetworkProtocol, Session> Sessions = new Dictionary<NetworkProtocol, Session>();
	public Action ShowNetLoading;
	public Action HideNetLoading;
    IPEndPoint ipEndPoint;
	public override void Init()
	{
        try
        {
            var addresses = Dns.GetHostAddresses(AppSettings.AppConfig.SvrGameIp);
            if (addresses == null || addresses.Length == 0)
            {
                Debug.LogError($"DNS resolve failed: no address for {AppSettings.AppConfig.SvrGameIp}");
                EventManager.Instance.MessageNotify(MessageConstBase.Msg_NetError, ErrorCore.ERR_ConnectError);
                return;
            }
            ipEndPoint = new IPEndPoint(addresses[0], AppSettings.AppConfig.SvrGamePort);
        }
        catch (Exception e)
        {
            Debug.LogError($"DNS resolve failed: {AppSettings.AppConfig.SvrGameIp}, {e.Message}");
            EventManager.Instance.MessageNotify(MessageConstBase.Msg_NetError, ErrorCore.ERR_ConnectError);
            return;
        }
        Session Session = Create(NetworkProtocol.TCP);
        Session.Create(NetworkProtocol.TCP, EServer.Login, ipEndPoint);
    }

    public void SetNetLoading(Action showNetLoading, Action hideNetLoading)
    {
        ShowNetLoading = showNetLoading;
        HideNetLoading = hideNetLoading;
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
			session.Value?.Update();
		}
    }

    public Session GetSession(NetworkProtocol networkProtocol)
	{
        if (!Sessions.TryGetValue(networkProtocol, out Session session))
		{
			return null;
		}
		return session;
    }

	public void SendMessage(long cmdid, object data)
	{
        Session session = GetSession(NetworkProtocol.TCP);
		if (session != null)
		{
			session.Send(EServer.Login, cmdid, data);
		}
    }
    public void Close()
    {
        Session session = GetSession(NetworkProtocol.TCP);
        if (session != null)
        {
            session.Dispose();
        }
        Sessions.Remove(NetworkProtocol.TCP);
    }

    public override void OnDestroy()
    {
        Close();
        base.OnDestroy();
    }
}