using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void AotMessageHandler(object[] msgDatas);
public class AotMessage : AotSingleton<AotMessage>
{
    private Dictionary<string, List<AotMessageHandler>> eventHandlerDic = new Dictionary<string, List<AotMessageHandler>>();

    public void RegisterMessageHandler(string eventName, AotMessageHandler message)
    {
        List<AotMessageHandler> list;
        if (!eventHandlerDic.TryGetValue(eventName, out list))
        {
            list = new List<AotMessageHandler>();
            eventHandlerDic.Add(eventName, list);
        }

        if (!list.Contains(message))
            list.Add(message);
    }

    public void RemoveMessage(string eventName)
    {
        if (eventHandlerDic.ContainsKey(eventName))
        {
            eventHandlerDic.Remove(eventName);
        }
    }

    public void MessageNotify(string eventName, params object[] msgData)
    {
        List<AotMessageHandler> handle;
        if (eventHandlerDic.TryGetValue(eventName, out handle))
        {
            foreach (AotMessageHandler itemHand in handle)
            {

                itemHand(msgData);
            }

        }
    }
}
