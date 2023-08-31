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


public class UIManager : MonoSingleton<UIManager>
{
    public GameObject canvasRoot;
    public Camera uiCamera;
    private Dictionary<string, GameObject> uiList = new Dictionary<string, GameObject>();
    private Transform baseCanvas;

    public override void Init()
    {
        canvasRoot = GameObject.Find("Canvas");
        //DontDestroyOnLoad(canvasRoot);
        uiCamera = GameObject.Find("Canvas/UICamera").GetComponent<Camera>();
        baseCanvas = GameObject.Find("Canvas/UICanvas/BaseCanvas").transform;
    }

    //public void PreLoad()
    //{
    //    // var seq = DG.Tweening.DOTween.Sequence();
    //    // seq.IsPlaying(true)

    //}

    public T GetUI<T>()
    {
        foreach (var item in uiList)
        {
            if (item.Key == typeof(T).Name)
            {
                return item.Value.GetComponent<T>();
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


    public void Open(string prefabName, params object[] args)
    {
        //require lua文件，得到返回的类
        LoadBaseUIAsync(string.Format("Prefab/UI/Panel/{0}", prefabName), (GameObject go) =>
        {
            go = GameObject.Instantiate(go);
            go.name = prefabName;
            go = ObjectOperation.SetParent(baseCanvas, go.transform).gameObject;
            Canvas cv = go.AddComponent<Canvas>();
            cv.overrideSorting = true;
            go.AddComponent<GraphicRaycaster>();
            OrderCanvas(go);
            uiList.Add(prefabName, go);
            go.GetComponent<BasePanel>().args = args;
            go.SetActive(true);
            
        });
    }

    public void Close(string prefabName)
    {
        GameObject go;
        if (uiList.TryGetValue(prefabName, out go))
        {
            GameObject.DestroyImmediate(go);
            uiList.Remove(prefabName);
        }
    }

    public void LoadBaseUIAsync(string prefabName, Action<GameObject> cb)
    {
        GameObject go;
        if (!uiList.TryGetValue(prefabName, out go))
        {
            ResManager.Instance.LoadAssetAsync(prefabName, prefabName + ".Prefab", typeof(GameObject), (UObject ugo) =>
            {
                go = ugo as GameObject;
                uiList.Add(prefabName, go);
                if (cb != null)
                    cb(go);
            });
        }
        else
        {
            if (cb != null)
                cb(go);
        }
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

    public void CloseAll()
    {
        List<GameObject> keys = new List<GameObject>();
        for (int i = baseCanvas.childCount - 1; i >= 0; i--)
        {
            keys.Add(baseCanvas.GetChild(i).gameObject);
        }

        for (int i = keys.Count - 1; i >= 0; i--)
        {
            GameObject go = keys[i].gameObject;
            keys.RemoveAt(i);
            uiList.Remove(go.name);
            DestroyImmediate(go);
        }
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
