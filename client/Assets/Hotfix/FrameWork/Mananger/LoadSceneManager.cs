using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.U2D;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Reflection;

public class LoadSceneManager : MonoSingleton<LoadSceneManager>
{
    // 加载进度
    float loadPro = 0;
    bool isfinish = false;
    // 用以接受异步加载的返回值
    AsyncOperation AsyncOp = null;

    string name;
    bool loading;
    object sceneScript;
    string oldName;
    object oldSceneScript;

    public void LoadScene(string sceneName, bool loading)
    {
        name = sceneName;
        this.loading = loading;
        if (this.loading)
        {
            UIManager.Instance.Open<LoadingPanel>();
        }
        ChangeScene(name);
    }

    public void ChangeScene(string name)
    {
        loadPro = 0;
        AsyncOp = null;
        Application.backgroundLoadingPriority = ThreadPriority.High;
        ResManager.Instance.LoadAssetAsync(name, "Scene/" + name + ".scene", typeof(Scene), (objt) =>
        {          
            AsyncOp = null;
            AsyncOp = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
            AsyncOp.allowSceneActivation = false;
            AsyncOp.completed += (AsyncOperation ao) => {              
                AsyncOp.allowSceneActivation = true;
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
                else {
                    Debug.LogError("No Scene Script");
                }
            };
        });
    }

    public string CurScene()
    {
        return name;
    }

    private void Update()
    {
        if (AsyncOp != null)//如果已经开始加载
        {
            loadPro = AsyncOp.progress; //获取加载进度,此处特别注意:加载场景的progress值最大为0.9!!!
        }
        if (loadPro >= 0.9f)//因为progress值最大为0.9,所以我们需要强制将其等于1
        {
            AsyncOp.allowSceneActivation = true;
        }
        
    }

    public void GC()
    {        
        ResManager.Instance.UnLoadAssetBundle(name);
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
    }
}
