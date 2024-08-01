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

    public void Create(NetworkProtocol networkProtocol, EServer eServer, string host, int port)
    {
        IPAddress[] addresses = Dns.GetHostAddresses(host);
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
        AService.Create((int)eServer, new IPEndPoint(addresses[0], port));
    }

    private void OnRead(long channelId, MemoryBuffer memoryBuffer)
    {
        var (id, data)  = protobufPacker.DeserializeFrom(memoryBuffer);
        if (AppSettings.AppConfig.DebugLog)
        {
            Type enumType = HybridCLRManager.Instance._hotUpdateAss.GetType(AppSettings.AppConfig.ProtoBuffPackageName + "SCMessageEnum");
            Debug.Log(AppSettings.AppConfig.ProtoBuffPackageName + Enum.GetName(enumType, id));
            Type dataType = HybridCLRManager.Instance._hotUpdateAss.GetType(AppSettings.AppConfig.ProtoBuffPackageName + Enum.GetName(enumType, id));
            object obj = ProtobufHelper.Deserialize(dataType, data, 0, data.Length);
            Debug.Log($"收到网络消息：{Enum.GetName(enumType, id)},{JsonConvert.SerializeObject(obj)}");            
        }
        EventManager.Instance.NetNotify(id, data);
    }

    public void Update()
    {
        AService.Update();
    }

    private void OnError(long channelId, int error)
    {
        
    }

    public void Send(EServer eserver, long messageEnum, object data)
    {
        if (AppSettings.AppConfig.DebugLog)
        {
            Type enumType = HybridCLRManager.Instance._hotUpdateAss.GetType(AppSettings.AppConfig.ProtoBuffPackageName + "CSMessageEnum");
            Debug.Log($"发送网络消息：{Enum.GetName(enumType, messageEnum)},{JsonConvert.SerializeObject(data)}");
        }
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