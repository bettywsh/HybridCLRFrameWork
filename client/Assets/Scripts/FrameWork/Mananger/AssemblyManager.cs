using Cysharp.Threading.Tasks;
using ET;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class AssemblyManager : Singleton<AssemblyManager>
{
    private readonly Dictionary<string, Type> allTypes = new();
    private readonly UnOrderMultiMapSet<Type, Type> types = new();
    Assembly[] assemblies;
    public async void Init(Assembly[] assemblies)
    { 
        this.assemblies = assemblies;
        await Init();
    }
    public override async UniTask Init()
    {
        await base.Init(); 
        Dictionary<string, Type> addTypes = GetAssemblyTypes(assemblies);
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
}
