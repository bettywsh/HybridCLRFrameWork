using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public static class EventHelper
{
    public static Dictionary<object, List<int>> DirMessages = new Dictionary<object, List<int>>();
    public static Dictionary<object, List<int>> DirNets = new Dictionary<object, List<int>>();
    public static Dictionary<object, List<int>> DirTimers = new Dictionary<object, List<int>>();

    public static void RegisterAllEvent(object obj, ReferenceCollector referenceCollector)
    {
        RegisterMessageEvent(obj);
        RegisterTimerEvent(obj);
        RegisterNetEvent(obj);
        RegisterUIEvent(obj, referenceCollector);
    }

    public static void RegisterMessageEvent(object obj)
    {
        var cache = AssemblyManager.Instance.GetEventCache(obj.GetType());
        foreach (var bind in cache.Messages)
        {
            EventManager.Instance.RegisterMessageHandler(bind.Id,
                new EventHandler()
                {
                    eventDelegate = BindEventDelegate(obj, bind.Method),
                    target = obj
                });
            AddId(DirMessages, obj, bind.Id);
        }
    }

    public static void RegisterTimerEvent(object obj)
    {
        var cache = AssemblyManager.Instance.GetEventCache(obj.GetType());
        foreach (var bind in cache.Timers)
        {
            EventManager.Instance.RegisterTimerHandler(bind.Id,
                new EventHandler()
                {
                    eventDelegate = BindEventDelegate(obj, bind.Method),
                    target = obj
                });
            AddId(DirTimers, obj, bind.Id);
        }
    }

    public static void RegisterNetEvent(object obj)
    {
        var cache = AssemblyManager.Instance.GetEventCache(obj.GetType());
        foreach (var bind in cache.Nets)
        {
            MessageDelegate messageDelegate = (MessageDelegate)Delegate.CreateDelegate(typeof(MessageDelegate), obj, bind.Method);
            EventManager.Instance.RegisterNetMessageHandler(bind.Id, messageDelegate);
            AddId(DirNets, obj, bind.Id);
        }
    }

    public static void RegisterUIEvent(object obj, ReferenceCollector referenceCollector)
    {
        if (referenceCollector == null)
        {
            Debug.LogError($"{obj.GetType().Name} 没有 ReferenceCollector");
            return;
        }
        var cache = AssemblyManager.Instance.GetEventCache(obj.GetType());
        foreach (var bind in cache.UIs)
        {
            if (bind.Kind == UIEventKind.Click)
            {
                var data = referenceCollector.Get(bind.Name);
                if (data == null || data.btnValue == null)
                {
                    Debug.LogError($"没有找到{bind.Name}属性定义的组件");
                    continue;
                }
                data.btnValue.onClick.RemoveAllListeners();
                data.btnValue.onClick.AddListener((UnityAction)Delegate.CreateDelegate(typeof(UnityAction), obj, bind.Method));
            }
            else if (bind.Kind == UIEventKind.Toggle)
            {
                var data = referenceCollector.Get(bind.Name);
                if (data == null || data.toggleValue == null)
                {
                    Debug.LogError($"没有找到{bind.Name}属性定义的组件");
                    continue;
                }
                data.toggleValue.onValueChanged.RemoveAllListeners();
                data.toggleValue.onValueChanged.AddListener((UnityAction<bool>)Delegate.CreateDelegate(typeof(UnityAction<bool>), obj, bind.Method));
            }
            else if (bind.Kind == UIEventKind.Slider)
            {
                var data = referenceCollector.Get(bind.Name);
                if (data == null || data.sliderValue == null)
                {
                    Debug.LogError($"没有找到{bind.Name}属性定义的组件");
                    continue;
                }
                data.sliderValue.onValueChanged.RemoveAllListeners();
                data.sliderValue.onValueChanged.AddListener((UnityAction<float>)Delegate.CreateDelegate(typeof(UnityAction<float>), obj, bind.Method));
            }
        }
    }

    public static void UnRegisterAllEvent(object obj)
    {
        if (DirMessages.TryGetValue(obj, out var messages))
        {
            foreach (var m in messages)
                EventManager.Instance.RemoveMessage(m, obj);
            DirMessages.Remove(obj);
        }

        if (DirTimers.TryGetValue(obj, out var timers))
        {
            foreach (var m in timers)
                EventManager.Instance.RemoveTimer(m, obj);
            DirTimers.Remove(obj);
        }

        if (DirNets.TryGetValue(obj, out var nets))
        {
            foreach (var n in nets)
                EventManager.Instance.RemoveNetMessage(n);
            DirNets.Remove(obj);
        }
    }

    static void AddId(Dictionary<object, List<int>> dic, object obj, int id)
    {
        if (!dic.TryGetValue(obj, out var list))
        {
            list = new List<int>();
            dic.Add(obj, list);
        }
        list.Add(id);
    }

    static readonly MethodInfo BindOneClassArgMethod =
        typeof(EventHelper).GetMethod(nameof(BindOneClassArgCore), BindingFlags.NonPublic | BindingFlags.Static);

    static EventDelegate BindEventDelegate(object target, MethodInfo method)
    {
        var ps = method.GetParameters();
        if (ps.Length == 0)
        {
            var act = (Action)Delegate.CreateDelegate(typeof(Action), target, method);
            return _ => act();
        }

        if (ps.Length == 1)
        {
            var p = ps[0].ParameterType;
            if (p == typeof(float))
            {
                var act = (Action<float>)Delegate.CreateDelegate(typeof(Action<float>), target, method);
                return args => act((float)args[0]);
            }
            if (p == typeof(int))
            {
                var act = (Action<int>)Delegate.CreateDelegate(typeof(Action<int>), target, method);
                return args => act((int)args[0]);
            }
            if (p == typeof(bool))
            {
                var act = (Action<bool>)Delegate.CreateDelegate(typeof(Action<bool>), target, method);
                return args => act((bool)args[0]);
            }
            if (!p.IsValueType)
                return BindOneClassArg(target, method, p);
        }

        return args => method.Invoke(target, args);
    }

    static EventDelegate BindOneClassArg(object target, MethodInfo method, Type argType)
    {
        return (EventDelegate)BindOneClassArgMethod.MakeGenericMethod(argType).Invoke(null, new object[] { target, method });
    }

    static EventDelegate BindOneClassArgCore<T>(object target, MethodInfo method) where T : class
    {
        var act = (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), target, method);
        return args => act(args != null && args.Length > 0 ? (T)args[0] : null);
    }
}
