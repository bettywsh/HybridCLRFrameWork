using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class EventHelper
{
    public static Dictionary<string, List<string>> dirMessages = new Dictionary<string, List<string>>();
    public static Dictionary<string, List<int>> dirNets = new Dictionary<string, List<int>>();
    public static void RegisterAllEvent(object obj, ReferenceCollector referenceCollector)
    {
        RegisterMessageEvent(obj);
        RegisterNetEvent(obj);
        RegisterUIEvent(obj, referenceCollector);
    }

    public static void RegisterMessageEvent(object obj)
    {
        Type type = obj.GetType();
        foreach (MethodInfo method in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
        {
            foreach (var att in method.GetCustomAttributes(true))
            {
                if (att is OnMessageAttribute)
                {
                    MessageManager.Instance.RegisterMessageHandler((att as OnMessageAttribute).Name, (msgDatas) => { method.Invoke(obj, null); });
                    List<string> messages;
                    if (!dirMessages.TryGetValue(type.Name, out messages))
                    {
                        messages = new List<string>();
                    }
                    messages.Add((att as OnMessageAttribute).Name);
                    dirMessages.Add(type.Name, messages);
                }
            }
        }
    }

    public static void RegisterNetEvent(object obj)
    {
        Type type = obj.GetType();
        foreach (MethodInfo method in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
        {
            foreach (var att in method.GetCustomAttributes(true))
            {
                if (att is OnNetAttribute)
                {
                    int id = (att as OnNetAttribute).Id;
                    MessageManager.Instance.RegisterNetMessageHandler(id, (msgDatas) => { method.Invoke(obj, null); });
                    List<int> nets;
                    if (!dirNets.TryGetValue(type.Name, out nets))
                    {
                        nets = new List<int>();
                    }
                    nets.Add(id);
                    dirNets.Add(type.Name, nets);
                }
            }
        }
    }

    public static void RegisterUIEvent(object obj, ReferenceCollector referenceCollector)
    {
        Type type = obj.GetType();
        foreach (MethodInfo method in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
        {
            foreach (var att in method.GetCustomAttributes(true))
            {
                if (att is OnClickAttribute)
                {
                    ReferenceData btn = referenceCollector.Get((att as OnClickAttribute).Name);
                    btn.btnValue.onClick.AddListener(() => { method.Invoke(obj, null); });
                    //clickList.Add(btn.btnValue);
                }
            }
        }
    }

    public static void UnRegisterAllEvent(object obj)
    {
        Type type = obj.GetType();
        List<string> messages;
        dirMessages.TryGetValue(type.Name, out messages);
        if (messages != null)
        {
            foreach (string m in messages)
            {
                MessageManager.Instance.RemoveMessage(m);
            }
            dirMessages.Remove(type.Name);
        }

        List<int> nets;
        dirNets.TryGetValue(type.Name, out nets);
        if (nets != null)
        {
            foreach (int n in nets)
            {
                MessageManager.Instance.RemoveNetMessage(n);
            }
            dirNets.Remove(type.Name);
        }
    }
}
