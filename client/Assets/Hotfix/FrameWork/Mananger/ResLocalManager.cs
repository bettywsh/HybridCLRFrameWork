using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;
using UnityEditor;
using Cysharp.Threading.Tasks;

public class ResLocalManager : MonoSingleton<ResLocalManager>
{
    public void LoadLocalUObjectAsync(string relativePath, Type type, Action<UObject> sharpFunc = null)
    {
        string assetName = ResPath.GetAssetPath(relativePath, type);
#if UNITY_EDITOR
        var obj = AssetDatabase.LoadAssetAtPath(assetName, type);
        if (sharpFunc != null)
        {
            sharpFunc(obj);
            sharpFunc = null;
        }
#endif
    }

    public UObject LoadLocalUObject(string relativePath, Type type)
    {
        string assetName = ResPath.GetAssetPath(relativePath, type);
#if UNITY_EDITOR
        var obj = AssetDatabase.LoadAssetAtPath(assetName, type);
        return obj;
#else
        return null;
#endif
    }

}
