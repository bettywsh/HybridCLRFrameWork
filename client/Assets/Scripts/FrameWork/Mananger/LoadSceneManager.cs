using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;
using YooAsset;
using Cysharp.Threading.Tasks;

public class LoadSceneManager : Singleton<LoadSceneManager>
{
    string name;
    bool loading;
    object sceneScript;
    string oldName;
    object oldSceneScript;
    Action openLoadingUI;
    public void Init(Action openUI = null)
    {
        openLoadingUI = openUI;
        name = SceneManager.GetActiveScene().name;
    }

    public void LoadScene(string scene, bool loading = false, bool isSendComplete = false)
    {
        oldName = name;
        name = scene;
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
        await ResManager.Instance.LoadSceneAsync("Assets/App/Scene/" + name);
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


    public void SetProgress(float progress)
    {
        MessageManager.Instance.MessageNotify("Msg_LoadingPanelProgress", progress);
    }

    public void SetComplete()
    {
        MessageManager.Instance.MessageNotify("Msg_LoadingPanelComplete");
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
