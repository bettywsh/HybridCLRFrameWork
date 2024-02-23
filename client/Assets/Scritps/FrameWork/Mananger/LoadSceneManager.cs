using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;
using YooAsset;
using Cysharp.Threading.Tasks;
using Codice.CM.SEIDInfo;

public class LoadSceneManager : Singleton<LoadSceneManager>
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
    Action openLoadingUI;
    public void Init(Action openUI)
    {
        openLoadingUI = openUI;
        name = "Start";
    }

    public void LoadScene(int scene, bool loading)
    {
        oldName = name;
        name = scene.ToString();
        this.loading = loading;
        if (this.loading)
        {
            openLoadingUI?.Invoke();
            //UIManager.Instance.Open<>();
        }
        UnLoadScene();
        ChangeScene(name);
    }

    void UnLoadScene()
    {
        loadPro = 0;
        Application.backgroundLoadingPriority = ThreadPriority.High;
        if (oldSceneScript != null)
        {
            BaseScene baseScene = oldSceneScript as BaseScene;
            baseScene.UnLoadScene();
        }
        GC();
    }

    public async void ChangeScene(string name)
    {
        var package = YooAssets.GetPackage(AppSettings.AppConfig.PackageName);
        handle = package.LoadSceneAsync("Assets/App/Scene/" + name + ".unity", LoadSceneMode.Single, false);
        await handle;
        handle = null;
        Type type = HybridCLRManager.Instance._hotUpdateAss.GetType(name + "Scene", false);
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
    }

    public void GC()
    {
        if (oldName != null)
        {
            ResManager.Instance.UnLoadAssetBundle(oldName);
        }
        System.GC.Collect();
        ResManager.Instance.GC();
        Resources.UnloadUnusedAssets();
    }
}
