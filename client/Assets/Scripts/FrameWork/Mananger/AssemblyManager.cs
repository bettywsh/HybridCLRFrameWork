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
    private Assembly[] assemblies;
    private Dictionary<string, Type> allPanel = new Dictionary<string, Type>();
    private Dictionary<string, Type> allSubPanel = new Dictionary<string, Type>();
    private Dictionary<string, Type> allCell = new Dictionary<string, Type>();
    private Dictionary<string, Type> allScene = new Dictionary<string, Type>();

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
        var types = GetTypes(typeof(CellAttribute));
        foreach (Type type in types)
        {
            allCell.Add(type.FullName, type);
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
            allScene.Add(type.FullName, type);
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
        }

        return t;
    }

}
