using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using DG.Tweening;
using UObject = UnityEngine.Object;
using Cysharp.Threading.Tasks;

public class UIManager : MonoSingleton<UIManager>
{
    public GameObject canvasRoot;
    public Camera uiCamera;
    private Dictionary<string, BasePanel> uiList = new Dictionary<string, BasePanel>();
    private Transform baseCanvas;

    public override void Init()
    {
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

    public T GetUI<T>() where T : BasePanel
    {
        foreach ((string name, BasePanel basePanel) in uiList)
        {
            if (name == typeof(T).Name)
            {
                return basePanel as T;
            }
        }
        return default;
    }

    //public void RefreshAllUI()
    //{
    //    for (int i = 0; i < baseCanvas.childCount; i++)
    //    {
    //        Transform tf = baseCanvas.GetChild(i);
    //        BasePanel basePanel = tf.GetComponent<BasePanel>();
    //        //basePanel.
    //    }
    //}


    public T Open<T>(params object[] args) where T : BasePanel
    {
        string prefabName = typeof(T).Name;
        BasePanel bp = null;
        T t = default;
        if (!uiList.TryGetValue(typeof(T).Name, out bp))
        {
            t = Activator.CreateInstance<T>();
            uiList.Add(typeof(T).Name, t as BasePanel);
            LoadPanel(typeof(T).Name, t as BasePanel, args);
        }
        else
        {
            t = bp as T;
        }      
        return t as T;
    }

    public async void LoadPanel(string name, BasePanel basePanel, params object[] args)
    {
        GameObject go = await ResManager.Instance.SceneLoadAssetAsync<GameObject>($"Assets/App/Prefab/UI/Panel/{name}.prefab");
        go = GameObject.Instantiate(go);
        go.name = name;
        go = ObjectHelper.SetParent(baseCanvas, go.transform).gameObject;
        Canvas cv = go.AddComponent<Canvas>();
        cv.overrideSorting = true;
        go.AddComponent<GraphicRaycaster>();
        OrderCanvas(go);
        basePanel.args = args;
        basePanel.transform = go.transform;
        basePanel?.OnBindEvent();
        basePanel?.OnOpen();
        go.SetActive(true);
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

    public void Close<T>() where T : BasePanel
    {
        string prefabName = typeof(T).Name;
        BasePanel obj;
        if (uiList.TryGetValue(typeof(T).Name, out obj))
        {
            BasePanel basePanel = obj;
            basePanel.OnClose();          
            GameObject.DestroyImmediate(basePanel.transform.gameObject);
            ResManager.Instance.UnLoadAssetBundle(prefabName);
            uiList.Remove(typeof(T).Name);
        }
    }

    public void CloseAll()
    {
        foreach ((string name, BasePanel basePanel) in uiList)
        {
            basePanel.OnClose();
            GameObject.DestroyImmediate(basePanel.transform.gameObject);
            uiList.Remove(name);
        }
    }

    private void Update()
    {
        foreach ((string name, BasePanel bp) in uiList)
        {
            if (bp.transform == null) { return; }
            BasePanel basePanel = bp;
            basePanel.OnUpdate();
        }
    }

    public Vector2 ScreenToUguiPos(Vector3 spos)
    {
        Vector2 outVec;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRoot.transform as RectTransform, spos, uiCamera, out outVec))
        {
            //Debug.Log("Setting anchored positiont to: " + outVec);
            //textRect.anchoredPosition = outVec;
        }
        return outVec;
    }


    public bool ClickUI()
    {

#if UNITY_EDITOR
        if (EventSystem.current.IsPointerOverGameObject())
#else
        if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
#endif
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
