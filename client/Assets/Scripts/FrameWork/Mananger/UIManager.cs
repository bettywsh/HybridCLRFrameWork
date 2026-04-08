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
using System.Threading.Tasks;

public class UIManager : MonoSingleton<UIManager>
{
    private GameObject canvasRoot;
    public Camera uiCamera;
    public Canvas uiCanvas;
    private Dictionary<string, PanelBase> uiList = new Dictionary<string, PanelBase>();
    private Transform baseCanvas;
    
    private Transform inputCanvas;
    private Transform inputEffect;
    public override async UniTask Init()
    {
        await base.Init();

        canvasRoot = GameObject.Find("Canvas");
        uiCamera = canvasRoot.transform.Find("UICamera").GetComponent<Camera>();
        baseCanvas = canvasRoot.transform.Find("UICanvas/BaseCanvas").transform;
        uiCanvas = canvasRoot.transform.Find("UICanvas").GetComponent<Canvas>();
        GameObject.DontDestroyOnLoad(canvasRoot);
        //初始化点击特效
        inputCanvas = canvasRoot.transform.Find("UICanvas/InputCanvas").transform;
        GameObject go = ResManager.Instance.CommonLoadAsset<GameObject>("Assets/App/Prefab/Effect/Fx_Click.prefab");
        inputEffect = GameObjectHelper.Instantiate(inputCanvas, go);
        inputEffect.gameObject.SetActive(false);
        inputEffect.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void CanvasScale()
    {
        float ScreenRatio = Screen.width / Screen.height;
        bool CanvasMatchWidth = ScreenRatio < 1.78f;
        //if (CanvasMatchWidth)
        //{
        //    float CanvasRealWidth = 1280;
        //    float CanvasRealHeight = 1280 / ScreenRatio;
        //    CanvasScaleToScreen = CanvasRealWidth / Screen.width;
        //}
        //else
        //{
        //    float CanvasRealHeight = 720;
        //    float CanvasRealWidth = 720 * ScreenRatio;
        //    CanvasScaleToScreen = CanvasRealHeight / Screen.height;
        //}
        GameObject.Find("Canvas/UICanvas").GetComponent<CanvasScaler>().matchWidthOrHeight = CanvasMatchWidth ? 0 : 1;
    }

    public void PreLoad()
    {

    }

    public T GetUI<T>() where T : PanelBase
    {
        foreach ((string name, PanelBase basePanel) in uiList)
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

    public async UniTask<Type> Open(Type type, params object[] args)
    {
        string prefabName = type.Name;
        object t;
        if (!uiList.TryGetValue(prefabName, out PanelBase bp))
        {
            t = Activator.CreateInstance(type);
            uiList.Add(prefabName, t as PanelBase);
            await LoadPanel(prefabName, t as PanelBase, args);
        }
        else
        {
            t = bp;
        }
        return t as Type;
    }

    public  async UniTask<T> Open<T>(params object[] args) where T : PanelBase
    {
        string prefabName = typeof(T).Name;
        PanelBase bp = null;
        T t = default;
        if (!uiList.TryGetValue(typeof(T).Name, out bp))
        {
            t = Activator.CreateInstance<T>();
            uiList.Add(typeof(T).Name, t as PanelBase);
            await LoadPanel(typeof(T).Name, t as PanelBase, args);
        }
        else
        {
            t = bp as T;
        }      
        return t as T;
    }
        

    public async UniTask LoadPanel(string name, PanelBase basePanel, params object[] args)
    {
        GameObject go = ResManager.Instance.SceneLoadAsset<GameObject>($"Assets/App/Prefab/UI/Panel/{name}.prefab");
        go = GameObject.Instantiate(go);
        go.name = name;
        go = GameObjectHelper.SetParent(baseCanvas, go.transform).gameObject;
        Canvas cv = go.AddComponent<Canvas>();
        cv.overrideSorting = true;
        go.AddComponent<GraphicRaycaster>();
        OrderCanvas(go);
        basePanel.args = args;
        basePanel.transform = go.transform;
        go.SetActive(true);
        basePanel.OnBindEvent();
        await basePanel.OnOpen();
    }

    void OrderCanvas(GameObject go)
    {
        int order = 0;
        for (int x = 0; x < baseCanvas.childCount; x++)
        {
            Transform tf = baseCanvas.GetChild(x);
            Canvas c = tf.GetComponent<Canvas>();
            if (c.sortingOrder >= 100)
            {
                continue;
            }
            if(c.sortingOrder> order)
                order = c.sortingOrder;
        }
        order += 5;
        go.GetComponent<Canvas>().sortingOrder = order;
        Canvas[] cs = go.transform.GetComponentsInChildren<Canvas>(true);
        for (int i = 0; i < cs.Length; i++)
        {
            if (cs[i].name != go.transform.name)
                cs[i].sortingOrder = order + cs[i].sortingOrder;
        }
        Renderer[] r = go.GetComponentsInChildren<Renderer>(true);
        for (int i = 0; i < r.Length; i++)
        {
            if (cs[i].name != go.transform.name)
                r[i].sortingOrder = order + r[i].sortingOrder;
        }

        //for (int x = 0; x < baseCanvas.childCount; x++)
        //{
        //    Transform tf = baseCanvas.GetChild(x);
        //    Canvas c = tf.GetComponent<Canvas>();
        //    if (c.sortingOrder >= 100)
        //    {
        //        continue;
        //    }
        //    int order = x * 5;
        //    c.sortingOrder = order;
        //    Canvas[] cs = tf.GetComponentsInChildren<Canvas>(false);
        //    for (int i = 0; i < cs.Length; i++)
        //    {
        //        if(cs[i].name != tf.name)
        //            cs[i].sortingOrder = order + cs[i].sortingOrder;
        //    }
        //    Renderer[] r = go.GetComponentsInChildren<Renderer>();
        //    for (int i = 0; i < r.Length; i++)
        //    {
        //        if (cs[i].name != tf.name)
        //            r[i].sortingOrder = order + r[i].sortingOrder;
        //    }
        //}
    }

    //框架用
    public void Close(Type type)
    {
        Close(type.Name);
    }

    public void Close<T>() where T : PanelBase
    {
        Close(typeof(T).Name);
    }

    void Close(string prefabName)
    {
        PanelBase obj;
        if (uiList.TryGetValue(prefabName, out obj))
        {
            PanelBase basePanel = obj;
            basePanel?.OnUnBindEvent();
            basePanel?.OnClose();
            basePanel?.Dispose();
            GameObject.DestroyImmediate(basePanel.transform.gameObject);
            uiList.Remove(prefabName);
        }
    }

    public void CloseAll()
    {
        foreach ((string name, PanelBase basePanel) in uiList)
        {
            basePanel?.OnClose();
            basePanel?.OnUnBindEvent();
            basePanel?.Dispose();
            Debug.LogError(basePanel.transform.name);
            GameObject.DestroyImmediate(basePanel.transform.gameObject);
        }
        uiList.Clear();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && inputEffect != null)
        {
            inputEffect.gameObject.SetActive(false);
            Vector2 pos = ScreenToUguiPos(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            RectTransform rect = inputEffect.transform as RectTransform;
            rect.anchoredPosition3D = new Vector3(pos.x, pos.y, 0);
            // TimerMgr.Instance.ClearTimer("btnClick");

            inputEffect.gameObject.SetActive(true);
            SoundManager.Instance.PlayEffectSound("Assets/App/Sound/UI/click1.mp3");
            // TimerMgr.Instance.SetTimer("btnClick", 1f, () => {
            //     effectClick.SetActive(false);
            // });
        }
        
        foreach ((string name, PanelBase bp) in uiList)
        {
            if (bp.transform == null) { return; }
            PanelBase basePanel = bp;
            basePanel?.OnUpdate();
        }
    }

    public Vector2 worldToUguiPos(Vector3 wpos)
    {
        return ScreenToUguiPos(RectTransformUtility.WorldToScreenPoint(uiCamera, wpos));
    }

    public Vector2 ScreenToUguiPos(Vector2 spos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(baseCanvas.transform as RectTransform, spos, uiCamera, out Vector2 outVec);
        return outVec;
    }

    public Vector2 ScreenToUguiPos(Vector3 spos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(baseCanvas.transform as RectTransform, spos, uiCamera, out Vector2 outVec);
        return outVec;
    }


    public bool GetClickUI()
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
