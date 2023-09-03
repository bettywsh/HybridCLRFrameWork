using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using UnityEngine;
using com.bochsler.protocol;

public class NetworkManager : MonoSingleton<NetworkManager>
{
	public AService Service { get; private set; }
	public Session Session { get; private set; }

	public IMessagePacker MessagePacker = new ProtobufPacker();
	public Action<int> OnConnect { get; set; }
	public Action<int> OnError { get; set; }

	//clinet
	public override void Init()
	{
        switch (AppConst.NetProtocol)
		{
			case NetworkProtocol.KCP:
				this.Service = new KService() { };
				break;
			case NetworkProtocol.TCP:
				this.Service = new TService(Packet.PacketSizeLength4) { };
				break;
			case NetworkProtocol.WebSocket:
				this.Service = new WService() { };
				break;
		}
		SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);

    }

	public void Connect(string ip, int port)
	{
		string address = ip + ":" + port;
		Connect(address);
	}

	public void Connect(string address)
	{
		AChannel channel = this.Service.ConnectChannel(address);
		Session = new Session(channel);
		Session.Start();
	}

	public void Update()
	{
		OneThreadSynchronizationContext.Instance.Update();
			
		if (this.Service == null)
		{
			return;
		}
			
		this.Service.Update();
	}

	public void Send<T>(CSMessageEnum messageEnum, T data)
	{
		Debug.Log(messageEnum.ToString() + ","+ LitJson.JsonMapper.ToJson(data));
		Send((int)messageEnum, ProtobufHelper.Serialize(data));
	}

	public void Send(int opcode, byte[] data)
	{
		Session.Send(MessagePacker.SerializeTo(opcode, data));
	}

	public void Recv(MemoryStream stream,int packetLength)
	{
		MessagePacker.DeserializeFrom(stream, packetLength);

    }

	public void DisConnect()
	{
        Service.Dispose();
    }
	
}