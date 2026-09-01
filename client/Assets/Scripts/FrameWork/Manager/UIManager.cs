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
    public override void Init()
    {
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

    public async UniTask<PanelBase> Open(Type type, params object[] args)
    {
        string prefabName = type.Name;
        if (!uiList.TryGetValue(prefabName, out PanelBase bp))
        {
            bp = Activator.CreateInstance(type) as PanelBase;
            uiList.Add(prefabName, bp);
            await LoadPanel(prefabName, bp, args);
        }
        return bp;
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
            Renderer[] r = cs[i].GetComponentsInChildren<Renderer>(true);
            for (int j = 0; j < r.Length; j++)
            {
                Canvas owner = r[j].GetComponentInParent<Canvas>();
                if (owner != cs[i])
                    continue;
                r[j].sortingOrder = cs[i].sortingOrder + r[j].sortingOrder;
            }
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
            GameObject.Destroy(basePanel.transform.gameObject);
            uiList.Remove(prefabName);
        }
    }

    public void CloseAll()
    {
        var names = new List<string>(uiList.Keys);
        for (int i = 0; i < names.Count; i++)
            Close(names[i]);
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
            //SoundManager.Instance.PlayEffectSound("Assets/App/Sound/UI/click1.mp3").Forget();
            // TimerMgr.Instance.SetTimer("btnClick", 1f, () => {
            //     effectClick.SetActive(false);
            // });
        }
        
        var names = new List<string>(uiList.Keys);
        for (int i = 0; i < names.Count; i++)
        {
            if (!uiList.TryGetValue(names[i], out PanelBase bp))
                continue;
            if (bp == null || bp.transform == null)
                continue;
            bp.OnUpdate();
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
        if (EventSystem.current == null)
            return false;
#if UNITY_EDITOR
        return EventSystem.current.IsPointerOverGameObject();
#else
        if (Input.touchCount <= 0)
            return false;
        return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
#endif
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
