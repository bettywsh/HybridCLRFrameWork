using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    Dictionary<Type, DataBase> configs = new Dictionary<Type, DataBase>();
    public override async UniTask Init()
    {
        await base.Init();
        //Assembly Hotfix = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Hotfix");
        var types = AssemblyManager.Instance.GetTypes(typeof(DataAttribute));
        foreach (Type type in types)
        {
            if (type.IsAbstract)
            {
                continue;
            }
            object[] objects = type.GetCustomAttributes(typeof(DataAttribute), true);

            foreach (object o in objects)
            {
                object obj = Activator.CreateInstance(type);
                DataBase baseData = obj as DataBase;      
                configs.Add(type, baseData);
            }
        }

        foreach (var k in configs)
        {
            k.Value.Init();
        }
    }

    public T GetData<T>() where T : DataBase
    {
        DataBase obj;
        configs.TryGetValue(typeof(T), out obj);
        return obj as T;
    }


    public void ResetAll()
    {
        foreach ((Type type, object obj) in configs)
        {
            ((DataBase)obj).Reset();
        }
        SDKManager.Instance.HideShop();
    }
}
