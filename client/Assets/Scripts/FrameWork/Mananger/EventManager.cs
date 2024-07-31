using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// xuss
/// 
/// 20200514
/// </summary>
public delegate void EventHandler(object[] msgDatas);
public class EventManager : Singleton<EventManager>
{
    private Dictionary<int, List<EventHandler>> netHandlerDic = new Dictionary<int, List<EventHandler>>();

    private Dictionary<int, List<EventHandler>> messageHandlerDic = new Dictionary<int, List<EventHandler>>();

    private Dictionary<int, List<EventHandler>> timerHandlerDic = new Dictionary<int, List<EventHandler>>();

    #region 网络消息
    public void RegisterNetMessageHandler(int cmdID, EventHandler message)
    {
        List<EventHandler> list;
        if (!netHandlerDic.TryGetValue(cmdID, out list))
        {
            list = new List<EventHandler>();
            netHandlerDic.Add(cmdID, list);
        }
        else {
            Debug.LogError($"{cmdID}事件已经被注册，不建议注册多个网络事件");
        }
        if (!list.Contains(message))
            list.Add(message);
    }

    public void RemoveNetMessage(int cmdID)
    {
        if (netHandlerDic.ContainsKey(cmdID))
        {
            netHandlerDic.Remove(cmdID);
        }
    }

    public void RemoveAllRegisterNet()
    {
        netHandlerDic.Clear();
    }

    public void NetNotify(int id, params object[] msgData)
    {
        List<EventHandler> handle;
        if (netHandlerDic.TryGetValue(id, out handle))
        {

            foreach (EventHandler itemHand in handle)
            {
                itemHand(msgData);
            }

        }
    }
    #endregion

    #region 逻辑消息
    public void RegisterMessageHandler(int eventName, EventHandler message)
    {
        List<EventHandler> list;
        if (!messageHandlerDic.TryGetValue(eventName, out list))
        {
            list = new List<EventHandler>();
            messageHandlerDic.Add(eventName, list);
        }

        if (!list.Contains(message))
            list.Add(message);
    }


    public void RemoveMessage(int eventName)
    {
        if (messageHandlerDic.ContainsKey(eventName))
        {
            messageHandlerDic.Remove(eventName);
        }
    }

    public void RemoveAllRegisterMessage()
    {
        messageHandlerDic.Clear();
    }

    public void MessageNotify(int eventName,params object[] msgData)
    {
        List<EventHandler> handle;

        if (messageHandlerDic.TryGetValue(eventName, out handle))
        {
            foreach (EventHandler itemHand in handle)
            {
                itemHand(msgData);
            }

        }
    }
    #endregion

    #region 定时器消息
    public void RegisterTimerHandler(int eventName, EventHandler message)
    {
        List<EventHandler> list;
        if (!timerHandlerDic.TryGetValue(eventName, out list))
        {
            list = new List<EventHandler>();
            timerHandlerDic.Add(eventName, list);
        }

        if (!list.Contains(message))
            list.Add(message);
    }


    public void RemoveTimer(int eventName)
    {
        if (timerHandlerDic.ContainsKey(eventName))
        {
            timerHandlerDic.Remove(eventName);
        }
    }

    public void RemoveAllRegisterTimer()
    {
        timerHandlerDic.Clear();
    }

    public void TimerNotify(int eventName, params object[] msgData)
    {
        List<EventHandler> handle;

        if (timerHandlerDic.TryGetValue(eventName, out handle))
        {
            foreach (EventHandler itemHand in handle)
            {
                itemHand(msgData);
            }

        }
    }
    #endregion
}
