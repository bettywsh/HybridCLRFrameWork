using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class EventHelper
{
    public static Dictionary<string, List<int>> DirMessages = new Dictionary<string, List<int>>();
    public static Dictionary<string, List<int>> DirNets = new Dictionary<string, List<int>>();
    public static Dictionary<string, List<int>> DirTimers = new Dictionary<string, List<int>>();
    public static void RegisterAllEvent(object obj, ReferenceCollector referenceCollector)
    {
        RegisterMessageEvent(obj);
        RegisterTimerEvent(obj);
        RegisterNetEvent(obj);
        RegisterUIEvent(obj, referenceCollector);
    }

    public static void RegisterMessageEvent(object obj)
    {
        var type = obj.GetType();
        var methods = AssemblyManager.Instance.GetMethods(type);
        foreach (MethodInfo method in methods)
        {
            foreach (var att in method.GetCustomAttributes(true))
            {
                if (att is not OnMessageAttribute) continue;
                EventManager.Instance.RegisterMessageHandler((att as OnMessageAttribute).Name,
                    new EventHandler() { eventDelegate = (msgDatas) => { method.Invoke(obj, msgDatas);},
                        type = type
                    });
                if (!DirMessages.TryGetValue(type.Name, out var messages))
                {
                    messages = new List<int>();
                    DirMessages.Add(type.Name, messages);
                }
                messages.Add((att as OnMessageAttribute).Name);
            }
        }
    }

    public static void RegisterTimerEvent(object obj)
    {
        var type = obj.GetType();
        var methods = AssemblyManager.Instance.GetMethods(type);
        foreach (MethodInfo method in methods)
        {
            foreach (var att in method.GetCustomAttributes(true))
            {
                if (att is not OnTimerAttribute) continue;
                EventManager.Instance.RegisterTimerHandler((att as OnTimerAttribute).Name,
                    new EventHandler() { eventDelegate = (msgDatas) => { method.Invoke(obj, msgDatas);
                    }});
                if (!DirTimers.TryGetValue(type.Name, out var timers))
                {
                    timers = new List<int>();
                    DirTimers.Add(type.Name, timers);
                }
                timers.Add((att as OnTimerAttribute).Name);
            }
        }
    }

    public static void RegisterNetEvent(object obj)
    {
        var type = obj.GetType();
        var methods = AssemblyManager.Instance.GetMethods(type);
        foreach (MethodInfo method in methods)
        {
            foreach (var att in method.GetCustomAttributes(true))
            {
                if (att is not OnNetAttribute) continue;
                var id = (att as OnNetAttribute).Id;
                EventManager.Instance.RegisterNetMessageHandler(id,
                    new EventHandler()
                    {
                        eventDelegate = (object[] msgDatas) =>
                        {
                            method.Invoke(obj, msgDatas);
                        }
                    });
                if (!DirNets.TryGetValue(type.Name, out var nets))
                {
                    nets = new List<int>();
                    DirNets.Add(type.Name, nets);
                }
                nets.Add(id);
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
                    var btn = referenceCollector.Get((att as OnClickAttribute).Name);
                    if (btn == null)
                    {
                        Debug.LogError($"没有找到{(att as OnClickAttribute).Name}属性定义的组件");
                    }
                    if (btn.btnValue == null)
                    {
                        Debug.LogError($"没有找到{(att as OnClickAttribute).Name}属性定义的组件");
                    }
                    btn.btnValue.onClick.RemoveAllListeners();
                    btn.btnValue.onClick.AddListener(() => { method.Invoke(obj, null); });
                }
                else if (att is OnToggleChangedAttribute)
                {
                    var btn = referenceCollector.Get((att as OnToggleChangedAttribute).Name);
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
        var type = obj.GetType();
        DirMessages.TryGetValue(type.Name, out var messages);
        if (messages != null)
        {
            foreach (var m in messages)
            {
                EventManager.Instance.RemoveMessage(m, obj.GetType());
            }
            DirMessages.Remove(type.Name);
        }

        DirTimers.TryGetValue(type.Name, out var timers);
        if (timers != null)
        {
            foreach (var m in timers)
            {
                EventManager.Instance.RemoveTimer(m, obj.GetType());
            }
            DirTimers.Remove(type.Name);
        }

        DirNets.TryGetValue(type.Name, out var nets);
        if (nets != null)
        {
            foreach (var n in nets)
            {
                EventManager.Instance.RemoveNetMessage(n);
            }
            DirNets.Remove(type.Name);
        }
    }
}
