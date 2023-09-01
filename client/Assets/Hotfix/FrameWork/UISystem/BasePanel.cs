using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    public object[] args;
    public virtual void Awake()
    {
        //×¢²áÊÂ¼þ
        MethodInfo[] MethodInfos = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public);
        for (int i = 0; i < MethodInfos.Length; i++)
        {
            if (MethodInfos[i].Name.Contains("Msg_"))
            {
                MessageHandler messageHandler = Delegate.CreateDelegate(typeof(MessageHandler), this, MethodInfos[i]) as MessageHandler;
                MessageManager.Instance.RegisterMessageHandler(MethodInfos[i].Name, messageHandler);
            }
        }
    }

    public virtual void Start()
    {


    }

    public virtual void OnDestroy()
    {


    }
}
