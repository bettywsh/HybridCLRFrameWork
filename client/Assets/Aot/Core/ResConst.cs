using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResConst
{
    /// <summary>
    /// 是否是打包模式
    /// </summary>
    /// 
#if UNITY_EDITOR
    public const bool IsABMode = false;
#else
    public const bool IsABMode = true;
#endif

    /// <summary>
    /// 热更新模式
    /// </summary>
#if UNITY_EDITOR
    public const bool UpdateModel = false;
#else
    public const bool UpdateModel = true;
#endif

    /// <summary>
    /// 热更下载地址
    /// </summary>
    public const string SvrResIp = "http://192.168.11.18:8082/";

    /// <summary>
    /// 框架根目录
    /// </summary>
    public const string RootFolderName = "App";

    /// <summary>
    /// 项目文件夹打包方式配置
    /// </summary>
    public const string BuildFolderName = "BuildConfig";

    /// <summary>
    /// 项目文件夹打包方式配置
    /// </summary>
    public const string BuildFile = "Build.json";

    /// <summary>
    /// Lua后缀
    /// </summary>
    public const string LuaExtName = ".lua.bytes";

    /// <summary>
    /// Prefab后缀
    /// </summary>
    public const string PrefabExtName = ".prefab";

    /// <summary>
    /// Atlas后缀
    /// </summary>
    public const string AtlasExtName = ".spriteatlas";

    /// <summary>
    /// Sprite后缀
    /// </summary>
    public const string TextureExtName = ".png";

    /// <summary>
    /// Material后缀
    /// </summary>
    public const string MaterialExtName = ".mat";

    /// <summary>
    /// Font后缀
    /// </summary>
    public const string FontExtName = ".ttf";

    /// <summary>
    /// Asset后缀
    /// </summary>
    public const string AssetExtName = ".asset";

    /// <summary>
    /// Scene后缀
    /// </summary>
    public const string SceneExtName = ".unity";

    /// <summary>
    /// AssetBunld后缀
    /// </summary>
    public const string AssetBunldExtName = ".unity3d";

    /// <summary>
    /// bytes后缀
    /// </summary>
    public const string BytesExtName = ".bytes";

    /// <summary>
    /// txt后缀
    /// </summary>
    public const string TxtExtName = ".txt";

    /// <summary>
    /// json后缀
    /// </summary>
    public const string JsonExtName = ".json";

    /// <summary>
    /// 版本文件
    /// </summary>
    public const string VerFile = "ver.txt";

    /// <summary>
    /// 版本文件列表
    /// </summary>
    public const string CheckFile = "files.txt";
}


