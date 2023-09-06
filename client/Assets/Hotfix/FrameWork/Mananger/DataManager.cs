using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DataManager : Singleton<DataManager>
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
                BaseData baseData = obj as BaseData;
                baseData.Init();
                configs.Add(type, baseData);
            }
        }
    }

    public T GetData<T>() where T : BaseData
    {
        object obj;
        configs.TryGetValue(typeof(T), out obj);
        return obj as T;
    }

}
