using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum UIEventKind
{
    Click,
    Toggle,
    Slider
}

public struct MethodEventBind
{
    public int Id;
    public MethodInfo Method;
}

public struct UIEventBind
{
    public UIEventKind Kind;
    public string Name;
    public MethodInfo Method;
}

public class TypeEventCache
{
    public readonly List<MethodEventBind> Messages = new();
    public readonly List<MethodEventBind> Timers = new();
    public readonly List<MethodEventBind> Nets = new();
    public readonly List<UIEventBind> UIs = new();

    public static readonly TypeEventCache Empty = new();
}

public class AssemblyManager : Singleton<AssemblyManager>
{
    private readonly Dictionary<string, Type> allTypes = new();
    private readonly UnOrderMultiMapSet<Type, Type> types = new();
    private readonly Dictionary<Type, TypeEventCache> eventCaches = new();
    private Assembly[] hotUpdateAss;
    private Dictionary<string, Type> allPanel = new Dictionary<string, Type>();
    private Dictionary<string, Type> allSubPanel = new Dictionary<string, Type>();
    private Dictionary<string, Type> allCell = new Dictionary<string, Type>();
    private Dictionary<string, Type> allScene = new Dictionary<string, Type>();
    private Dictionary<string, Type> allData = new Dictionary<string, Type>();
    public override void Init()
    {
        hotUpdateAss = new Assembly[1] { HybridCLRManager.Instance._hotUpdateAss };
        Dictionary<string, Type> addTypes = GetAssemblyTypes(hotUpdateAss);
        foreach ((string fullName, Type type) in addTypes)
        {
            this.allTypes[fullName] = type;
            if (type.IsAbstract)
            {
                continue;
            }
            // 记录所有的有BaseAttribute标记的的类型
            object[] objects = type.GetCustomAttributes(typeof(BaseAttribute), true);

            foreach (object o in objects)
            {
                this.types.Add(o.GetType(), type);
            }
            CacheTypeEvents(type);
        }
        var types = GetTypes(typeof(CellAttribute));
        foreach (Type type in types)
        {
            allCell.Add(type.Name, type);
        }

        types = GetTypes(typeof(PanelAttribute));
        foreach (Type type in types)
        {
            allPanel.Add(type.FullName, type);
        }

        types = GetTypes(typeof(SubPanelAttribute));
        foreach (Type type in types)
        {
            allSubPanel.Add(type.FullName, type);
        }

        types = GetTypes(typeof(SceneAttribute));
        foreach (Type type in types)
        {
            allScene.Add(type.Name, type);
        }
        types = GetTypes(typeof(DataAttribute));
        foreach (Type type in types)
        {
            allData.Add(type.FullName, type);
        }
    }

    Dictionary<string, Type> GetAssemblyTypes(params Assembly[] args)
    {
        Dictionary<string, Type> mTypes = new Dictionary<string, Type>();

        foreach (Assembly ass in args)
        {
            foreach (Type type in ass.GetTypes())
            {
                mTypes[type.FullName] = type;
            }
        }

        return mTypes;
    }

    public HashSet<Type> GetTypes(Type systemAttributeType)
    {
        if (!this.types.ContainsKey(systemAttributeType))
        {
            return new HashSet<Type>();
        }

        return this.types[systemAttributeType];
    }

    public Type GetType(EAttribute eattr, string name)
    {
        Type t = null;
        switch (eattr)
        {
            case EAttribute.Cell:
                this.allCell.TryGetValue(name, out t);
                break;
            case EAttribute.Panel:
                this.allPanel.TryGetValue(name, out t);
                break;
            case EAttribute.SubPanel:
                this.allSubPanel.TryGetValue(name, out t);
                break;
            case EAttribute.Scene:
                this.allScene.TryGetValue(name, out t);
                break;
            case EAttribute.Data:
                this.allData.TryGetValue(name, out t);
                break;
        }

        return t;
    }

    public TypeEventCache GetEventCache(Type typeClass)
    {
        if (typeClass == null)
            return TypeEventCache.Empty;
        if (eventCaches.TryGetValue(typeClass, out var cache))
            return cache;
        return TypeEventCache.Empty;
    }

    void CacheTypeEvents(Type type)
    {
        TypeEventCache cache = null;
        foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
        {
            var attrs = methodInfo.GetCustomAttributes(true);
            if (attrs.Length == 0)
                continue;

            foreach (var attr in attrs)
            {
                if (attr is OnMessageAttribute messageAtt)
                {
                    cache ??= new TypeEventCache();
                    cache.Messages.Add(new MethodEventBind { Id = messageAtt.Name, Method = methodInfo });
                }
                else if (attr is OnTimerAttribute timerAtt)
                {
                    cache ??= new TypeEventCache();
                    cache.Timers.Add(new MethodEventBind { Id = timerAtt.Name, Method = methodInfo });
                }
                else if (attr is OnNetAttribute netAtt)
                {
                    cache ??= new TypeEventCache();
                    cache.Nets.Add(new MethodEventBind { Id = netAtt.Id, Method = methodInfo });
                }
                else if (attr is OnClickAttribute clickAtt)
                {
                    cache ??= new TypeEventCache();
                    cache.UIs.Add(new UIEventBind { Kind = UIEventKind.Click, Name = clickAtt.Name, Method = methodInfo });
                }
                else if (attr is OnToggleChangedAttribute toggleAtt)
                {
                    cache ??= new TypeEventCache();
                    cache.UIs.Add(new UIEventBind { Kind = UIEventKind.Toggle, Name = toggleAtt.Name, Method = methodInfo });
                }
                else if (attr is OnSliderChangedAttribute sliderAtt)
                {
                    cache ??= new TypeEventCache();
                    cache.UIs.Add(new UIEventBind { Kind = UIEventKind.Slider, Name = sliderAtt.Name, Method = methodInfo });
                }
            }
        }

        if (cache != null)
            eventCaches.Add(type, cache);
    }
}
