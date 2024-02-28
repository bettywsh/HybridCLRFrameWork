using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Linq;
using static UnityEditor.Progress;

[RequireComponent(typeof(UnityEngine.UI.RectMask2D))]
[RequireComponent(typeof(UnityEngine.UI.LoopScrollRect))]
[DisallowMultipleComponent]
public class ListView : MonoBehaviour, LoopScrollPrefabSource, LoopScrollDataSource
{
    public GameObject Item;
    private int mtotalCount = -1;

    public Action<int, Transform> OnItemRender;
    public int TotalCount
    {
        get
        {
            return mtotalCount;
        }
        set
        {
            mtotalCount = value;
            var ls = GetComponent<LoopScrollRect>();
            ls.prefabSource = this;
            ls.dataSource = this;
            ls.totalCount = mtotalCount;
            ls.RefillCells();
        }
    }

    // Implement your own Cache Pool here. The following is just for example.
    Stack<Transform> pool = new Stack<Transform>();
    public GameObject GetObject(int index)
    {
        if (pool.Count == 0)
        {
            GameObject go = Instantiate(Item);
            Type type = HybridCLRManager.Instance._hotUpdateAss.GetType(Item.name, false);
            go.AddComponent(type);
            BaseCell baseCell = go.GetComponent<BaseCell>();
            baseCell.transform = go.transform;
            baseCell.OnBindEvent();
            return go;
        }
        Transform candidate = pool.Pop();
        candidate.gameObject.SetActive(true);
        return candidate.gameObject;
    }

    public void ReturnObject(Transform trans)
    {
        // Use `DestroyImmediate` here if you don't need Pool
        //trans.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
        trans.gameObject.SetActive(false);
        trans.SetParent(transform, false);
        pool.Push(trans);
    }


    public async void ProvideData(Transform transform, int idx)
    {
        OnItemRender(idx, transform);
    }

}
