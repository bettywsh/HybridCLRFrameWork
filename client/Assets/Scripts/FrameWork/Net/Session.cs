using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Newtonsoft.Json;

public sealed class Session : IDisposable
{
    public int Id { get;  set; }
    public AService AService { get; set; }

    public ProtobufPacker protobufPacker = new ProtobufPacker();

    public Session(int id, NetworkProtocol networkProtocol)
    {
        Id = id;
    }

    public void Create(NetworkProtocol networkProtocol, EServer eServer, IPEndPoint ipEndPoint)
    {
        //IPAddress[] addresses = Dns.GetHostAddresses(host);
        switch (networkProtocol)
        {
            case NetworkProtocol.KCP:
                //this.Service = new KService() { };
                break;
            case NetworkProtocol.TCP:
                AService = new TService() { };
                break;
            case NetworkProtocol.WebSocket:
                AService = new WService();
                break;
        }
        AService.ReadCallback = OnRead;
        AService.ErrorCallback = OnError;
        try
        {
            AService.Create((int)eServer, ipEndPoint);
        }
        catch {
            OnError((int)eServer, ErrorCore.ERR_ConnectError);
        }
    }

    public void Connect()
    { 
    
    }

    private void OnRead(long channelId, MemoryBuffer memoryBuffer)
    {
        var (id, data)  = protobufPacker.DeserializeFrom(memoryBuffer);
        if (!NoNetLoadingMsgIds.Contains(id))
            NetworkManager.Instance.HideNetLoading?.Invoke();
        if (AppSettings.AppConfig.DebugLog)
        {
            Type enumType = HybridCLRManager.Instance._hotUpdateAss.GetType(AppSettings.AppConfig.ProtoBuffPackageName + "MsgID");
            Debug.Log(AppSettings.AppConfig.ProtoBuffPackageName + Enum.GetName(enumType, id));
            string className = AppSettings.AppConfig.ProtoBuffPackageName + Enum.GetName(enumType, id);
            Type dataType = HybridCLRManager.Instance._hotUpdateAss.GetType(className.Replace("MSG",""));
            object obj = ProtobufHelper.Deserialize(dataType, data, 0, data.Length);
            Debug.Log($"收到网络消息：{Enum.GetName(enumType, id)},{JsonConvert.SerializeObject(obj)}");            
        }
        EventManager.Instance.NetNotify(id, data);
        //重置心跳包时间
        //EventManager.Instance.MessageNotify(107);
    }

    public void Update()
    {
        AService.Update();
    }

    private void OnError(long channelId, int error)
    {
        EventManager.Instance.MessageNotify(109, error);
        NetworkManager.Instance.HideNetLoading?.Invoke();
    }

    public void Send(EServer eserver, long messageEnum, object data)
    {
        if (AppSettings.AppConfig.DebugLog )
        {
            Type enumType = HybridCLRManager.Instance._hotUpdateAss.GetType(AppSettings.AppConfig.ProtoBuffPackageName + "MsgID");
            Debug.Log($"发送网络消息：{Enum.GetName(enumType, messageEnum)},{JsonConvert.SerializeObject(data)}");
        }
        if (!NoNetLoadingMsgIds.Contains(messageEnum))
            NetworkManager.Instance.ShowNetLoading?.Invoke();
        Send(eserver, (long)messageEnum, ProtobufHelper.Serialize(data));
    }

    public void Send(EServer eserver, long messageEnum, byte[] buffers)
	{
        AService.Send((long)eserver, protobufPacker.SerializeTo(messageEnum, buffers));
    }

    public void Dispose()
    {
        AService.Dispose();
    }
}