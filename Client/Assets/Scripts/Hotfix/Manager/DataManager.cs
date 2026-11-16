using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    Dictionary<Type, DataBase> configs = new();
    public override void Init()
    {
        //Assembly Hotfix = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == "Hotfix");
        var types = AssemblyManager.Instance.GetTypes(typeof(DataAttribute));
        foreach (Type type in types)
        {
            object obj = Activator.CreateInstance(type);
            DataBase baseData = obj as DataBase;
            configs.Add(type, baseData);
        }

        foreach (var k in configs)
        {
            k.Value.Init();
        }
    }

    public T GetData<T>() where T : DataBase
    {
        configs.TryGetValue(typeof(T), out DataBase obj);
        return obj as T;
    }


    // public void Reset()
    // {
    //     foreach (var k in configs)
    //     {
    //         k.Value.Reset();
    //     }
    // }

    public override void Dispose()
    {
        foreach (var k in configs)
        {
            k.Value.Dispose();
        }
        configs.Clear();
   
        base.Dispose();
    }
}
