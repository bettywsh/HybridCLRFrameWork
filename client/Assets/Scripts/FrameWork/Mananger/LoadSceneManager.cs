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
    SceneBase sceneScript;
    string oldName;
    SceneBase oldSceneScript;
    public override async UniTask Init()
    {
        await base.Init();
        name = SceneManager.GetActiveScene().name;
    }

    public void LoadScene(string scene, bool isSendComplete = false)
    {
        oldName = name;
        name = scene;
        if (sceneScript != null)
        {
            oldSceneScript = sceneScript;
        }
        UnLoadScene();
        ChangeScene(name);
    }

    void UnLoadScene()
    {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        if (oldSceneScript != null)
        {
            oldSceneScript.UnLoadScene();
        }
        GC();
    }

    public async void ChangeScene(string name)
    {
        await ResManager.Instance.LoadSceneAsync("Assets/App/Scene/" + name);
        Type type = HybridCLRManager.Instance._hotUpdateAss.GetType(name + "Scene", false);
        if (type != null)
        {
            sceneScript = Activator.CreateInstance(type) as SceneBase;
            sceneScript.LoadScene();
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
