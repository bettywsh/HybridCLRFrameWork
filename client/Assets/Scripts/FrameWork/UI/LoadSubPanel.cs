using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSubPanel : MonoBehaviour
{

    SubPanelBase subPanelBase;
    public async UniTask<T> Open<T>(params object[] args)
    {
        if (subPanelBase != null)
        {
            Close();
        }
        string prefabName = typeof(T).Name;
        T t = Activator.CreateInstance<T>();
        GameObject go = await ResManager.Instance.SceneLoadAssetAsync<GameObject>($"Assets/App/Prefab/UI/SubPanel/{prefabName}.prefab");
        go = GameObject.Instantiate(go);
        go.name = prefabName;
        go = GameObjectHelper.SetParent(transform, go.transform).gameObject;
        Canvas cv = go.AddComponent<Canvas>();
        cv.overrideSorting = true;
        go.AddComponent<GraphicRaycaster>();
        OrderCanvas();
        subPanelBase = t as SubPanelBase;
        subPanelBase.args = args;
        subPanelBase.transform = go.transform;
        go.SetActive(true);
        subPanelBase?.OnBindEvent();
        subPanelBase?.OnOpen();
        return t;
    }

    void OrderCanvas()
    {
        int order = transform.GetComponentInParent<Canvas>().sortingOrder;
        Canvas[] cs = transform.GetComponentsInChildren<Canvas>(false);
        for (int i = 0; i < cs.Length; i++)
        {
            cs[i].sortingOrder = order + cs[i].sortingOrder;
        }
        Renderer[] r = transform.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < r.Length; i++)
        {
            r[i].sortingOrder = order + r[i].sortingOrder;
        }
    }

    public void Close()
    {
        if (subPanelBase != null)
        {
            GameObject.DestroyImmediate(subPanelBase.transform.gameObject);
        }
    }

    private void OnDestroy()
    {
        subPanelBase?.Close();
    }
}
