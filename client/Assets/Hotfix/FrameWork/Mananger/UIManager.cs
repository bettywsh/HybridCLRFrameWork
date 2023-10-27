using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using DG.Tweening;
using UObject = UnityEngine.Object;
using TMPro;
using Cysharp.Threading.Tasks;

public class UIManager : MonoSingleton<UIManager>
{
    public GameObject canvasRoot;
    public Camera uiCamera;
    private Dictionary<string, object> uiList = new Dictionary<string, object>();
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

        //    // var seq = DG.Tweening.DOTween.Sequence();
        //    // seq.IsPlaying(true)

    }

public T GetUI<T>() where T : BasePanel
    {
        foreach (var item in uiList)
        {
            if (item.Key == typeof(T).Name)
            {
                return item.Value as T;
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


    public async UniTask<T> Open<T>(params object[] args) where T : BasePanel
    {
        string prefabName = typeof(T).Name;
        if (uiList.ContainsKey(typeof(T).Name))
            return default;
        T t = Activator.CreateInstance<T>();
        GameObject go = await ResManager.Instance.SceneLoadAssetAsync<GameObject>($"Assets/App/Prefab/UI/Panel/{prefabName}{ResConst.PrefabExtName}");
        go = GameObject.Instantiate(go);
        go.name = prefabName;
        go = ObjectHelper.SetParent(baseCanvas, go.transform).gameObject;
        Canvas cv = go.AddComponent<Canvas>();
        cv.overrideSorting = true;
        go.AddComponent<GraphicRaycaster>();
        OrderCanvas(go);
        uiList.Add(typeof(T).Name, t);
        BasePanel basePanel = t as BasePanel;
        basePanel.args = args;
        basePanel.transform = go.transform;
        basePanel?.OnBindEvent();
        basePanel?.OnOpen();
        go.SetActive(true);
        return t;
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
        object obj;
        if (uiList.TryGetValue(typeof(T).Name, out obj))
        {
            BasePanel basePanel = obj as BasePanel;
            basePanel.OnClose();          
            GameObject.DestroyImmediate(basePanel.transform.gameObject);
            ResManager.Instance.UnLoadAssetBundle(prefabName);
            uiList.Remove(typeof(T).Name);
        }
    }

    public void CloseAll()
    {
        foreach ((string k, object v) in uiList)
        {
            BasePanel basePanel = v as BasePanel;
            basePanel.OnClose();
            GameObject.DestroyImmediate(basePanel.transform.gameObject);
            ResManager.Instance.UnLoadAssetBundle(k);
            uiList.Remove(k);
        }
    }

    private void Update()
    {
        foreach ((string name, object obj) in uiList)
        {
            BasePanel basePanel = obj as BasePanel;
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
