using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class AotUIManager : AotSingleton<AotUIManager>
{
    public GameObject canvasRoot;
    public Camera uiCamera;
    private Dictionary<string, AotPanelBase> uiList = new Dictionary<string, AotPanelBase>();
    private Transform baseCanvas;

    public override async UniTask Init()
    {
        await base.Init();
        canvasRoot = GameObject.Find("Canvas");
        GameObject.DontDestroyOnLoad(canvasRoot);
        uiCamera = GameObject.Find("Canvas/UICamera").GetComponent<Camera>();
        baseCanvas = GameObject.Find("Canvas/UICanvas/BaseCanvas").transform;
        CanvasScale();
    }

    public void CanvasScale()
    {
        float ScreenRatio = Screen.width / Screen.height;
        bool CanvasMatchWidth = ScreenRatio < 1.78f;
        if (CanvasMatchWidth)
        {
            float CanvasRealWidth = 1280;
            float CanvasRealHeight = 1280 / ScreenRatio;
            //CanvasScaleToScreen = CanvasRealWidth / Screen.width;
        }
        else
        {
            float CanvasRealHeight = 720;
            float CanvasRealWidth = 720 * ScreenRatio;
            //CanvasScaleToScreen = CanvasRealHeight / Screen.height;
        }
        GameObject.Find("Canvas/UICanvas").GetComponent<CanvasScaler>().matchWidthOrHeight = CanvasMatchWidth ? 0 : 1;
    }

    public void PreLoad()
    {

    }


    public T Open<T>(params object[] args) where T : AotPanelBase
    {
        AotPanelBase bp = null;
        T t = default;
        if (!uiList.TryGetValue(typeof(T).Name, out bp))
        {
            LoadPanel(typeof(T).Name, args);
        }
        else
        {
            t = bp as T;
        }      
        return t as T;
    }
        

    public async void LoadPanel(string name, params object[] args)
    {
        GameObject go = await AotResManager.Instance.SceneLoadAssetAsync<GameObject>(SceneManager.GetActiveScene().name, $"Assets/App/Prefab/AotUI/{name}.prefab");
        go = GameObject.Instantiate(go);
        go.name = name;
        go.transform.SetParent(baseCanvas, false);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        go.transform.localEulerAngles = Vector3.zero;
        Canvas cv = go.AddComponent<Canvas>();
        cv.overrideSorting = true;
        go.AddComponent<GraphicRaycaster>();
        OrderCanvas(go);
        AotPanelBase basePanel = go.GetComponent<AotPanelBase>();
        basePanel.args = args;
        basePanel.transform = go.transform;
        go.SetActive(true);
        uiList.Add(name, basePanel);
    }

    void OrderCanvas(GameObject go)
    {
        for (int x = 0; x < baseCanvas.childCount; x++)
        {
            Transform tf = baseCanvas.GetChild(x);
            Canvas c = tf.GetComponent<Canvas>();
            if (c.sortingOrder >= 100)
            {
                continue;
            }
            int order = x * 5;
            c.sortingOrder = order;
            Canvas[] cs = tf.GetComponentsInChildren<Canvas>(false);
            for (int i = 0; i < cs.Length; i++)
            {
                cs[i].sortingOrder = order + cs[i].sortingOrder;
            }
            Renderer[] r = go.GetComponentsInChildren<Renderer>();
            for (int i = 0; i < r.Length; i++)
            {
                r[i].sortingOrder = order + r[i].sortingOrder;
            }
        }
    }

    //框架用
    public void Close(Type type)
    {
        Close(type.Name);
    }

    public void Close<T>() where T : AotPanelBase
    {
        Close(typeof(T).Name);
    }

    void Close(string prefabName)
    {
        AotPanelBase obj;
        if (uiList.TryGetValue(prefabName, out obj))
        {
            AotPanelBase basePanel = obj;
            GameObject.DestroyImmediate(basePanel.transform.gameObject);
            uiList.Remove(prefabName);
        }
    }
}
