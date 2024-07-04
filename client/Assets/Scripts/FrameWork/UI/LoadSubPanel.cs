using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSubPanel : MonoBehaviour
{

    public PanelBase panelBase;
    public async UniTask<T> Load<T>(string subPanelName, params object[] args)
    {
        string prefabName = typeof(T).Name;
        T t = Activator.CreateInstance<T>();
        GameObject go = await ResManager.Instance.SceneLoadAssetAsync<GameObject>($"Assets/App/Prefab/UI/SubPanel/{prefabName}.prefab");
        go = GameObject.Instantiate(go);
        go.name = prefabName;
        go = GameObjectHelper.SetParent(transform, go.transform).gameObject;
        Canvas cv = go.AddComponent<Canvas>();
        cv.overrideSorting = true;
        go.AddComponent<GraphicRaycaster>();
        panelBase = t as PanelBase;
        panelBase.args = args;
        panelBase.transform = go.transform;
        go.SetActive(true);
        panelBase?.OnBindEvent();
        panelBase?.OnOpen();
        return t;
    }

    private void OnDestroy()
    {
        panelBase?.OnClose();
    }
}
