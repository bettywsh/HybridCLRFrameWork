using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBase : IDisposable
{
    public virtual void LoadScene()
    {
        //if (SceneManager.GetActiveScene().name != LoadSceneManager.Instance.CurScene())
        //{
            //LoadSceneManager.Instance.UnLoadScene();
        //}
    }

    public virtual void UnLoadScene()
    {

    }

    public virtual void Dispose()
    {

    }
}
