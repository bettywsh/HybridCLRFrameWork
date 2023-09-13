using com.bochsler.protocol;
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
    public static void RegisterEvent(object obj)
    {
        Type type = obj.GetType();
        //×¢²áÊÂ¼þ
        MethodInfo[] MethodInfos = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        for (int i = 0; i < MethodInfos.Length; i++)
        {
            if (MethodInfos[i].Name.Contains("Msg_"))
            {
                MessageHandler messageHandler = Delegate.CreateDelegate(typeof(MessageHandler), obj, MethodInfos[i]) as MessageHandler;
                MessageManager.Instance.RegisterMessageHandler(MethodInfos[i].Name, messageHandler);
                List<string> messages;
                if (!dirMessages.TryGetValue(type.Name, out messages))
                {
                    messages = new List<string>();
                }
                messages.Add(MethodInfos[i].Name);
                dirMessages.Add(type.Name, messages);

            }
            else if (MethodInfos[i].Name.Contains("Click_"))
            {
                try
                {
                    string btnStr = MethodInfos[i].Name.Replace("Click_", "");
                    UnityAction cb = Delegate.CreateDelegate(typeof(UnityAction), obj, MethodInfos[i]) as UnityAction;
                    object v = type.GetField("view").GetValue(obj);
                    Button btn = (Button)v.GetType().GetField(btnStr).GetValue(v);
                    btn.onClick.AddListener(cb);
                }
                catch
                {
                    Debug.LogError(MethodInfos[i].Name + ",Not Find Buttom Or Reference");
                }
            }
            else if (MethodInfos[i].Name.Contains("Net_"))
            {
                string net = MethodInfos[i].Name.Replace("Net_", "");
                SCMessageEnum sc = (SCMessageEnum)Enum.Parse(typeof(SCMessageEnum), net);
                MessageHandler messageHandler = Delegate.CreateDelegate(typeof(MessageHandler), obj, MethodInfos[i]) as MessageHandler;
                MessageManager.Instance.RegisterNetMessageHandler((int)sc, messageHandler);
                List<int> nets;
                if (!dirNets.TryGetValue(type.Name, out nets))
                {
                    nets = new List<int>();
                }
                nets.Add((int)sc);
                dirNets.Add(type.Name, nets);
            }
        }
    }

    public static void UnRegisterEvent(object obj)
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
