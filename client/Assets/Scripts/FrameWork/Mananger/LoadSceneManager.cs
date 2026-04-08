using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using YooAsset;
using Cysharp.Threading.Tasks;

public class LoadSceneManager : Singleton<LoadSceneManager>
{
    string name = SceneManager.GetActiveScene().name;
    SceneBase sceneScript;
    string oldName;
    SceneBase oldSceneScript;
    int msgProgress;
    int msgComplete;
    SceneHandle curSceneHandle;
    public override async UniTask Init()
    {
        await base.Init();
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

    public async void UnLoadScene()
    {
        //if (name == oldName)
        //    return;
        Application.backgroundLoadingPriority = ThreadPriority.High;
        if (curSceneHandle != null)
        {
            var operation = curSceneHandle.UnloadAsync();
            await operation;
        }
        oldSceneScript?.UnLoadScene();
        GC();
    }

    public async void ChangeScene(string name)
    {
        curSceneHandle = ResManager.Instance.LoadSceneAsync("Assets/App/Scene/" + name);

        //SceneManager.LoadScene(name);
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
        //System.GC.Collect();
        ResManager.Instance.GC();
        //Resources.UnloadUnusedAssets();
    }
}
