using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSubPanel : MonoBehaviour
{
    public async UniTask<T> Load<T>(string subPanelName, params object[] args)
    {
        string prefabName = typeof(T).Name;
        T t = Activator.CreateInstance<T>();
        GameObject go = await ResManager.Instance.SceneLoadAssetAsync<GameObject>($"Assets/App/Prefab/UI/SubPanel/{prefabName}{ResConst.PrefabExtName}");
        go = GameObject.Instantiate(go);
        go.name = prefabName;
        go = ObjectHelper.SetParent(transform, go.transform).gameObject;
        Canvas cv = go.AddComponent<Canvas>();
        cv.overrideSorting = true;
        go.AddComponent<GraphicRaycaster>();
        BasePanel basePanel = t as BasePanel;
        basePanel.args = args;
        basePanel.transform = go.transform;
        basePanel?.OnBindEvent();
        basePanel?.OnOpen();
        go.SetActive(true);
        return t;
    }
}
