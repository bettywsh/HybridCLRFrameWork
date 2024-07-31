using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;
using YooAsset;
using Cysharp.Threading.Tasks;
using System.Runtime.Remoting.Messaging;

public class LoadSceneManager : Singleton<LoadSceneManager>
{
    string name;
    SceneBase sceneScript;
    string oldName;
    SceneBase oldSceneScript;
    int msgProgress;
    int msgComplete;
    public override async UniTask Init()
    {
        await base.Init();
        name = SceneManager.GetActiveScene().name;
    }

    public async UniTask Init(int progress, int complete)
    {
        msgProgress = progress;
        msgComplete = complete;
        await Init();
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
        Type type = AssemblyManager.Instance.GetType(EAttribute.Scene, name + "Scene");
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
        EventManager.Instance.MessageNotify(msgProgress, progress);
    }

    public void SetComplete()
    {
        EventManager.Instance.MessageNotify(msgComplete);
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
