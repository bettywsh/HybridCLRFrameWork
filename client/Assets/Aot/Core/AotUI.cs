using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AotUI : AotSingleton<AotUI>
{
    private Transform baseCanvas;
    private Dictionary<string, GameObject> uiList = new Dictionary<string, GameObject>();

    public void Open(string prefabName, params object[] args)
    {
        string abName = "App/Prefab/AotUI/" + prefabName + ".unity3d";
        string assetName = "Assets/App/Prefab/AotUI/" + prefabName + ".prefab";
        baseCanvas = GameObject.Find("Canvas/UICanvas/BaseCanvas").transform;
        GameObject go = AotRes.Instance.LoadAsset<GameObject>(abName, assetName);
        go = GameObject.Instantiate(go);
        go.name = prefabName;
        go = SetParent(baseCanvas, go.transform).gameObject;
        Canvas cv = go.AddComponent<Canvas>();
        cv.overrideSorting = true;
        go.AddComponent<GraphicRaycaster>();
        uiList.Add(prefabName, go);
        OrderCanvas(go);
        go.GetComponent<AotBasePanel>().args = args;
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

    public void Close(string prefabName)
    {
        GameObject go;
        if (uiList.TryGetValue(prefabName, out go))
        {
            GameObject.DestroyImmediate(go);
            uiList.Remove(prefabName);
        }
    }

    public static Transform SetParent(Transform parent, Transform go)
    {
        go.SetParent(parent, false);
        go.localPosition = Vector3.zero;
        go.localScale = Vector3.one;
        go.localEulerAngles = Vector3.zero;
        return go;
    }
}
