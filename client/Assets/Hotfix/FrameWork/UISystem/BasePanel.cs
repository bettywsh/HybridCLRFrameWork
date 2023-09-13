using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using com.bochsler.protocol;


public class BasePanel
{
    public Transform transform;
    public object[] args;
    
    public List<string> messages = new List<string>();
    public List<int> nets = new List<int>();
    public void RegisterEvent()
    {
        //×¢²áÊÂ¼þ
        MethodInfo[] MethodInfos = GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        for (int i = 0; i < MethodInfos.Length; i++)
        {
            if (MethodInfos[i].Name.Contains("Msg_"))
            {
                MessageHandler messageHandler = Delegate.CreateDelegate(typeof(MessageHandler), this, MethodInfos[i]) as MessageHandler;
                MessageManager.Instance.RegisterMessageHandler(MethodInfos[i].Name, messageHandler);
                messages.Add(MethodInfos[i].Name);
            }
            else if (MethodInfos[i].Name.Contains("Click_"))
            {
                try
                {
                    string btnStr = MethodInfos[i].Name.Replace("Click_", "");
                    UnityAction cb = Delegate.CreateDelegate(typeof(UnityAction), this, MethodInfos[i]) as UnityAction;
                    object v = GetType().GetField("view").GetValue(this);
                    Button btn = (Button)v.GetType().GetField(btnStr).GetValue(v);
                    btn.onClick.AddListener(cb);
                }
                catch {
                    Debug.LogError(MethodInfos[i].Name + ",Not Find Buttom Or Reference");
                }
            }
            else if(MethodInfos[i].Name.Contains("Net_"))
            {
                string net = MethodInfos[i].Name.Replace("Net_", "");
                SCMessageEnum sc = (SCMessageEnum)Enum.Parse(typeof(SCMessageEnum), net);
                MessageHandler messageHandler = Delegate.CreateDelegate(typeof(MessageHandler), this, MethodInfos[i]) as MessageHandler;
                MessageManager.Instance.RegisterNetMessageHandler((int)sc, messageHandler);
                nets.Add((int)sc);
            }
        }
    }

    public virtual void OnBindEvent()
    {
        RegisterEvent();
    }

    public virtual void OnOpen()
    {

    }

    public virtual void OnUpdate()
    { 
    
    }

    public virtual void OnClose()
    {
        foreach (string m in messages)
        {
            MessageManager.Instance.RemoveMessage(m);
        }
        foreach (int n in nets)
        {
            MessageManager.Instance.RemoveNetMessage(n);
        }
        messages.Clear();
        nets.Clear();
    }
}
