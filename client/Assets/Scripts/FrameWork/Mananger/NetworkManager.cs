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
	public Action showNetLoading;
	public Action hideNetLoading;
    public bool isShowNetLoading = false;
    IPEndPoint ipEndPoint;
    public void Init(Action showNetLoading, Action hideNetLoading)
	{
		this.showNetLoading = showNetLoading;
		this.hideNetLoading = hideNetLoading;
        try
        {
            var ipaddress = Dns.GetHostAddresses(AppSettings.AppConfig.SvrGameIp)[0];
            ipEndPoint = new IPEndPoint(ipaddress, AppSettings.AppConfig.SvrGamePort);
        }
        catch { }

    }
	public override async UniTask Init()
	{
        await base.Init();
        Session Session = Create(NetworkProtocol.TCP);
        Session.Create(NetworkProtocol.TCP, EServer.Login, ipEndPoint);
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
        isShowNetLoading = false;
    }

    private void OnDestroy()
    {
        Close();
    }
}