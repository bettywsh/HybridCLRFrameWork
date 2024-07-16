using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


public class WChannel : AChannel
{
    public HttpListenerWebSocketContext WebSocketContext { get; }

    private readonly WService Service;

    private readonly WebSocket webSocket;

    private readonly Queue<MemoryBuffer> queue = new();

    private bool isSending;

    private bool isConnected;

    private CancellationTokenSource cancellationTokenSource = new();

    public WChannel(long id, WebSocket webSocket, IPEndPoint ipEndPoint, WService service)
    {
        this.Service = service;
        this.Id = id;
        this.ChannelType = ChannelType.Connect;
        this.webSocket = webSocket;

        isConnected = false;

        this.Service.ThreadSynchronizationContext.Post(() => this.ConnectAsync($"ws://{ipEndPoint}").Forget());
    }

    public override void Dispose()
    {
        if (this.IsDisposed)
        {
            return;
        }

        this.cancellationTokenSource.Cancel();
        this.cancellationTokenSource.Dispose();
        this.cancellationTokenSource = null;

        this.webSocket.Dispose();
    }

    private async UniTask ConnectAsync(string url)
    {
        try
        {
            await ((ClientWebSocket)this.webSocket).ConnectAsync(new Uri(url), cancellationTokenSource.Token);
            isConnected = true;
            this.StartRecv().Forget();
            this.StartSend().Forget();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            this.OnError(ErrorCore.ERR_WebsocketConnectError);
        }
    }

    public void Send(MemoryBuffer memoryBuffer)
    {
        this.queue.Enqueue(memoryBuffer);

        if (this.isConnected)
        {
            this.StartSend().Forget();
        }
    }

    private async UniTask StartSend()
    {
        if (this.IsDisposed)
        {
            return;
        }

        try
        {
            if (this.isSending)
            {
                return;
            }

            this.isSending = true;

            while (true)
            {
                if (this.queue.Count == 0)
                {
                    this.isSending = false;
                    return;
                }

                MemoryBuffer stream = this.queue.Dequeue();

                try
                {
                    await this.webSocket.SendAsync(stream.GetMemory(), WebSocketMessageType.Binary, true, cancellationTokenSource.Token);
                    this.Service.Recycle(stream);

                    if (this.IsDisposed)
                    {
                        return;
                    }
                }
                catch (TaskCanceledException e)
                {
                    Debug.LogWarning(e.ToString());
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    this.OnError(ErrorCore.ERR_WebsocketSendError);
                    return;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    private readonly byte[] cache = new byte[ushort.MaxValue];

    public async UniTask StartRecv()
    {
        if (this.IsDisposed)
        {
            return;
        }

        try
        {
            while (true)
            {
                ValueWebSocketReceiveResult receiveResult;
                int receiveCount = 0;
                do
                {                    
                    receiveResult = await this.webSocket.ReceiveAsync(
                                       new Memory<byte>(cache, receiveCount, this.cache.Length - receiveCount),
                                       cancellationTokenSource.Token);
                    if (this.IsDisposed)
                    {
                        return;
                    }        
                    receiveCount += receiveResult.Count;
                }
                while (!receiveResult.EndOfMessage);

                if (receiveResult.MessageType == WebSocketMessageType.Close)
                {
                    this.OnError(ErrorCore.ERR_WebsocketPeerReset);
                    return;
                }

                if (receiveResult.Count > ushort.MaxValue)
                {
                    await this.webSocket.CloseAsync(WebSocketCloseStatus.MessageTooBig, $"message too big: {receiveCount}",
                        cancellationTokenSource.Token);
                    this.OnError(ErrorCore.ERR_WebsocketMessageTooBig);
                    return;
                }
                MemoryBuffer memoryBuffer = this.Service.Fetch(receiveCount);
                memoryBuffer.SetLength(receiveCount);
                memoryBuffer.Seek(0, SeekOrigin.Begin);
                Array.Copy(this.cache, 0, memoryBuffer.GetBuffer(), 0, receiveCount);
                this.OnRead(memoryBuffer);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            this.OnError(ErrorCore.ERR_WebsocketRecvError);
        }
    }

    private void OnRead(MemoryBuffer memoryStream)
    {
        try
        {
            this.Service.ReadCallback(this.Id, memoryStream);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            this.OnError(ErrorCore.ERR_PacketParserError);
        }
    }

    private void OnError(int error)
    {
        Debug.Log($"WChannel error: {error} {this.RemoteAddress}");

        long channelId = this.Id;

        this.Service.Remove(channelId);

        this.Service.ErrorCallback(channelId, error);
    }
}