using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class EventHelper
{
    public static Dictionary<string, List<int>> dirMessages = new Dictionary<string, List<int>>();
    public static Dictionary<string, List<int>> dirNets = new Dictionary<string, List<int>>();
    public static Dictionary<string, List<int>> dirTimers = new Dictionary<string, List<int>>();
    public static void RegisterAllEvent(object obj, ReferenceCollector referenceCollector)
    {
        RegisterMessageEvent(obj);
        RegisterTimerEvent(obj);
        RegisterNetEvent(obj);
        RegisterUIEvent(obj, referenceCollector);
    }

    public static void RegisterMessageEvent(object obj)
    {
        Type type = obj.GetType();
        var methods = AssemblyManager.Instance.GetMethods(type);
        foreach (MethodInfo method in methods)
        {
            foreach (var att in method.GetCustomAttributes(true))
            {
                if (att is OnMessageAttribute)
                {
                    EventManager.Instance.RegisterMessageHandler((att as OnMessageAttribute).Name, (msgDatas) => { method.Invoke(obj, msgDatas); });
                    List<int> messages;
                    if (!dirMessages.TryGetValue(type.Name, out messages))
                    {
                        messages = new List<int>();
                        dirMessages.Add(type.Name, messages);
                    }
                    messages.Add((att as OnMessageAttribute).Name);                    
                }
            }
        }
    }

    public static void RegisterTimerEvent(object obj)
    {
        Type type = obj.GetType();
        var methods = AssemblyManager.Instance.GetMethods(type);
        foreach (MethodInfo method in methods)
        {
            foreach (var att in method.GetCustomAttributes(true))
            {
                if (att is OnTimerAttribute)
                {
                    EventManager.Instance.RegisterTimerHandler((att as OnTimerAttribute).Name, (msgDatas) => { method.Invoke(obj, msgDatas); });
                    List<int> timers;
                    if (!dirTimers.TryGetValue(type.Name, out timers))
                    {
                        timers = new List<int>();
                        dirTimers.Add(type.Name, timers);
                    }
                    timers.Add((att as OnTimerAttribute).Name);
                }
            }
        }
    }

    public static void RegisterNetEvent(object obj)
    {
        Type type = obj.GetType();
        var methods = AssemblyManager.Instance.GetMethods(type);
        foreach (MethodInfo method in methods)
        {
            foreach (var att in method.GetCustomAttributes(true))
            {
                if (att is OnNetAttribute)
                {
                    int id = (att as OnNetAttribute).Id;
                    EventManager.Instance.RegisterNetMessageHandler(id, (object[] msgDatas) => {
                        method.Invoke(obj, msgDatas);
                    });
                    List<int> nets;
                    if (!dirNets.TryGetValue(type.Name, out nets))
                    {
                        nets = new List<int>();
                        dirNets.Add(type.Name, nets);
                    }
                    nets.Add(id);                    
                }
            }
        }
    }

    public static void RegisterUIEvent(object obj, ReferenceCollector referenceCollector)
    {
        var methods = AssemblyManager.Instance.GetMethods(obj.GetType());
        foreach (MethodInfo method in methods)
        {
            foreach (var att in method.GetCustomAttributes(true))
            {
                if (att is OnClickAttribute)
                {
                    ReferenceData btn = referenceCollector.Get((att as OnClickAttribute).Name);
                    if (btn.btnValue == null)
                    {
                        Debug.LogError($"没有找到{(att as OnClickAttribute).Name}属性定义的组件");
                    }
                    btn.btnValue.onClick.RemoveAllListeners();
                    btn.btnValue.onClick.AddListener(() => { method.Invoke(obj, null); });
                }
                else if (att is OnToggleChangedAttribute)
                {
                    ReferenceData btn = referenceCollector.Get((att as OnToggleChangedAttribute).Name);
                    if (btn.toggleValue == null)
                    {
                        Debug.LogError($"没有找到{(att as OnClickAttribute).Name}属性定义的组件");
                    }
                    btn.toggleValue.onValueChanged.RemoveAllListeners();
                    btn.toggleValue.onValueChanged.AddListener((bool select) => { method.Invoke(obj, new object[1] { select }); });
                }
                else if (att is OnSliderChangedAttribute)
                {
                    ReferenceData btn = referenceCollector.Get((att as OnSliderChangedAttribute).Name);
                    if (btn.sliderValue == null)
                    {
                        Debug.LogError($"没有找到{(att as OnClickAttribute).Name}属性定义的组件");
                    }
                    btn.sliderValue.onValueChanged.RemoveAllListeners();
                    btn.sliderValue.onValueChanged.AddListener((float value) => { method.Invoke(obj, new object[1] { value }); });
                }
            }
        }
    }

    public static void UnRegisterAllEvent(object obj)
    {
        Type type = obj.GetType();
        List<int> messages;
        dirMessages.TryGetValue(type.Name, out messages);
        if (messages != null)
        {
            foreach (int m in messages)
            {
                EventManager.Instance.RemoveMessage(m);
            }
            dirMessages.Remove(type.Name);
        }

        List<int> timers;
        dirTimers.TryGetValue(type.Name, out timers);
        if (timers != null)
        {
            foreach (int m in timers)
            {
                EventManager.Instance.RemoveMessage(m);
            }
            dirTimers.Remove(type.Name);
        }

        List<int> nets;
        dirNets.TryGetValue(type.Name, out nets);
        if (nets != null)
        {
            foreach (int n in nets)
            {
                EventManager.Instance.RemoveNetMessage(n);
            }
            dirNets.Remove(type.Name);
        }
    }
}
