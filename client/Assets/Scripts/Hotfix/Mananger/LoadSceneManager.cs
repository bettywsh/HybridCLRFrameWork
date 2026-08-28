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
    SceneHandle curSceneHandle;
    public override void Init()
    {

    }

    public async UniTask LoadScene(string scene, bool isSendComplete = false)
    {
        oldName = name;
        name = scene;
        if (sceneScript != null)
        {
            oldSceneScript = sceneScript;
        }
        await UnLoadScene();
        await ChangeScene(name);
    }

    public async UniTask UnLoadScene()
    {
        //if (name == oldName)
        //    return;
        Application.backgroundLoadingPriority = ThreadPriority.High;
        if (curSceneHandle != null)
        {
            var operation = curSceneHandle.UnloadAsync();
            await operation.ToUniTask();
        }
        oldSceneScript?.UnLoadScene();
        GC();
    }

    public async UniTask ChangeScene(string name)
    {
        curSceneHandle = ResManager.Instance.LoadSceneAsync("Assets/App/Scene/" + name);
        await curSceneHandle.ToUniTask();
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
        EventManager.Instance.MessageNotify(MessageConst.Msg_LoadingPanelProgress, progress);
    }

    public void SetComplete()
    {
        EventManager.Instance.MessageNotify(MessageConst.Msg_LoadingPanelComplete);
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
