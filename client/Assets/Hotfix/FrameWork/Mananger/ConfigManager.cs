using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class ConfigManager : Singleton<ConfigManager>
{

    Dictionary<Type, object> configs = new Dictionary<Type, object>();

    public override void Init()
    {
        Assembly Hotfix = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Hotfix");
        foreach (Type type in Hotfix.GetTypes())
        {
            if (type.IsAbstract)
            {
                continue;
            }
            object[] objects = type.GetCustomAttributes(typeof(ConfigAttribute), true);

            foreach (object o in objects)
            {
                object obj = Activator.CreateInstance(type);
                BaseConfig baseConfig = obj as BaseConfig;
                baseConfig.Init();
                configs.Add(type, baseConfig);
            }
        }
    }

    public T LoadConfig<T>() where T : BaseConfig
    {
        object obj;
        configs.TryGetValue(typeof(T), out obj);
        return obj as T;
    }

    public Type LoadConfig(Type type)
    {
        object obj;
        configs.TryGetValue(type.GetType(), out obj);
        return obj as Type;
    }
}

