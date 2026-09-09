using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EventHandler
{
    public EventDelegate eventDelegate;
    public object target;
}

public delegate void MessageDelegate(byte[] msgDatas);
public delegate void EventDelegate(params object[] msgDatas);
public class EventManager : Singleton<EventManager>
{
    private Dictionary<int, MessageDelegate> messageHandlerDic = new();

    private Dictionary<int, List<EventHandler>> eventHandlerDic = new();

    private Dictionary<int, List<EventHandler>> timerEventHandlerDic = new();

    #region 网络消息
    public void RegisterNetMessageHandler(int cmdID, MessageDelegate message)
    {
        if (!messageHandlerDic.ContainsKey(cmdID))
        {
            messageHandlerDic.Add(cmdID, message);
        }
        else
        {
            Debug.LogError($"{cmdID}事件已经被注册，不建议注册多个网络事件");
        }
    }

    public void RemoveNetMessage(int cmdID)
    {
        if (messageHandlerDic.ContainsKey(cmdID))
        {
            messageHandlerDic.Remove(cmdID);
        }
    }

    public void RemoveAllRegisterNet()
    {
        messageHandlerDic.Clear();
    }

    public void NetNotify(int id, byte[] msgData)
    {
        if (messageHandlerDic.TryGetValue(id, out MessageDelegate message))
        {
            message(msgData);
        }
    }
    #endregion

    #region 逻辑消息
    public void RegisterMessageHandler(int eventName, EventHandler message)
    {
        if (!eventHandlerDic.TryGetValue(eventName, out List<EventHandler> list))
        {
            list = new List<EventHandler>();
            eventHandlerDic.Add(eventName, list);
        }

        if (!list.Contains(message))
            list.Add(message);
    }


    public void RemoveMessage(int eventName, object target)
    {
        if (!eventHandlerDic.TryGetValue(eventName, out var list))
            return;
        list.RemoveAll(x => ReferenceEquals(x.target, target));
        if (list.Count == 0)
            eventHandlerDic.Remove(eventName);
    }

    public void RemoveAllRegisterMessage()
    {
        eventHandlerDic.Clear();
    }

    public void MessageNotify(int eventName,params object[] msgData)
    {

        if (eventHandlerDic.TryGetValue(eventName, out List<EventHandler> handle))
        {
            for (int i = handle.Count - 1; i >= 0; i--)
            {
                handle[i].eventDelegate(msgData);
            }
        }
    }
    #endregion

    #region 定时器消息
    public void RegisterTimerHandler(int eventName, EventHandler message)
    {
        if (!timerEventHandlerDic.TryGetValue(eventName, out List<EventHandler> list))
        {
            list = new List<EventHandler>();
            timerEventHandlerDic.Add(eventName, list);
        }

        if (!list.Contains(message))
            list.Add(message);
    }


    public void RemoveTimer(int eventName, object target)
    {
        if (!timerEventHandlerDic.TryGetValue(eventName, out var list))
            return;
        list.RemoveAll(x => ReferenceEquals(x.target, target));
        if (list.Count == 0)
            timerEventHandlerDic.Remove(eventName);
    }

    public void RemoveAllRegisterTimer()
    {
        timerEventHandlerDic.Clear();
    }

    public void TimerNotify(int eventName, params object[] msgData)
    {

        if (timerEventHandlerDic.TryGetValue(eventName, out List<EventHandler> handle))
        {
            for (int i = handle.Count - 1; i >= 0; i--)
            {
                handle[i].eventDelegate(msgData);
            }
        }
    }
    #endregion
}
