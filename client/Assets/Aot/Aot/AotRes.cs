using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AotRes : AotSingleton<AotRes>
{
    Dictionary<string, AssetBundle> abDic = new Dictionary<string, AssetBundle>();
    public T LoadAsset<T>(string abName, string assetName) where T : UnityEngine.Object
    {
        if (ResConst.IsABMode)
        {
            string path = GetPersistentPath() + abName.ToLower();
            //if (!File.Exists(path))
            //{
            //    path = GetStreamingAssetsPath() + abName.ToLower();
            //}
            AssetBundle ab;
            if (!abDic.TryGetValue(abName, out ab))
            {
                ab = AssetBundle.LoadFromFile(path);
                abDic.Add(abName, ab);
            }
            return ab.LoadAsset<T>(assetName);
        }
        else
        {
#if UNITY_EDITOR
            return AssetDatabase.LoadAssetAtPath<T>(assetName);
#else
            return null;
#endif
        }
    }

    public void UnLoadAssetBundle()
    {
        List<KeyValuePair<string, UnityEngine.AssetBundle>> abs = abDic.ToList();
        for (int i = abs.Count - 1; i >= 0; i--)
        {
            abs[i].Value.Unload(false);
        }
        abDic.Clear();
        abDic = null;
    }


    string GetPersistentPath()
    {
        return $"{Application.persistentDataPath}/";
    }

//    string GetStreamingAssetsPath()
//    {
//#if UNITY_EDITOR
//        return Application.streamingAssetsPath + "/";
//#else
//#if UNITY_ANDROID
//        return Application.streamingAssetsPath + "/";      
//#elif UNITY_IOS
//        return Application.dataPath + @"/Raw";        

//#else
//        return Application.streamingAssetsPath + "/";
//#endif
//#endif
//    }
}
