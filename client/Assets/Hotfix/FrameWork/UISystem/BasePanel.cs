using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using com.bochsler.protocol;


public class BasePanel : MonoBehaviour
{
    public object[] args;
    public virtual void Awake()
    {
        RegisterEvent();
    }

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
            }
            else if (MethodInfos[i].Name.Contains("Click_"))
            {
                string btn = MethodInfos[i].Name.Replace("Click_", "");
                UnityAction cb = Delegate.CreateDelegate(typeof(UnityAction), this, MethodInfos[i]) as UnityAction;
                this[btn].onClick.AddListener(cb);

            }
            else if(MethodInfos[i].Name.Contains("Net_"))
            {
                string net = MethodInfos[i].Name.Replace("Net_", "");
                SCMessageEnum sc = (SCMessageEnum)Enum.Parse(typeof(SCMessageEnum), net);
                MessageHandler messageHandler = Delegate.CreateDelegate(typeof(MessageHandler), this, MethodInfos[i]) as MessageHandler;
                MessageManager.Instance.RegisterNetMessageHandler((int)sc, messageHandler);
            }
        }
    }

    public Button this[string name]
    {
        get
        {
            FieldInfo fi = GetType().GetField(name);
            object obj = fi.GetValue(this);
            return (Button)obj;
        }
        set
        {
            GetType().GetField(name).SetValue(this, value);
        }
    }

    public virtual void Start()
    {


    }

    public virtual void OnDestroy()
    {


    }
}
