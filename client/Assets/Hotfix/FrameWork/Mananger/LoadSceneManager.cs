using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Reflection;
using static UnityEditor.FilePathAttribute;
using static UnityEngine.UI.CanvasScaler;
using YooAsset;
using Cysharp.Threading.Tasks;

public class LoadSceneManager : MonoSingleton<LoadSceneManager>
{
    // 加载进度
    float loadPro = 0;
    bool isfinish = false;
    // 用以接受异步加载的返回值
    SceneHandle handle = null;

    string name;
    bool loading;
    object sceneScript;
    string oldName;
    object oldSceneScript;

    public async void LoadScene(string sceneName, bool loading)
    {
        name = sceneName;
        this.loading = loading;
        if (this.loading)
        {
            await UIManager.Instance.Open<LoadingPanel>();
        }
        ChangeScene(name);
    }

    public async void ChangeScene(string name)
    {
        loadPro = 0;
        Application.backgroundLoadingPriority = ThreadPriority.High;
        var package = YooAssets.GetPackage(App.AppConfig.PackageName);
        SceneHandle handle = package.LoadSceneAsync("Assets/App/Scene/" + name + ".unity", LoadSceneMode.Single, false);
        await handle;
        if (sceneScript != null)
        {
            BaseScene baseScene = sceneScript as BaseScene;
            baseScene.UnLoadScene();
        }
        GC();
        Type type = AotHybridCLR.Instance._hotUpdateAss.GetType(name + "Scene", false);
        if (type != null)
        {
            sceneScript = Activator.CreateInstance(type);
            BaseScene baseScene = sceneScript as BaseScene;
            baseScene.LoadScene();
        }
        else
        {
            Debug.LogError("No Scene Script");
        }
    }

    public string CurScene()
    {
        return name;
    }

    private void Update()
    {
        if (handle != null)//如果已经开始加载
        {
            loadPro = handle.Progress; //获取加载进度,此处特别注意:加载场景的progress值最大为0.9!!!
        }
        //if (loadPro >= 0.9f)//因为progress值最大为0.9,所以我们需要强制将其等于1
        //{
        //    handle.allowSceneActivation = true;
        //}
        
    }

    public void GC()
    {        
        ResManager.Instance.UnLoadAssetBundle(name);
        System.GC.Collect();
        Resources.UnloadUnusedAssets();        
    }
}
