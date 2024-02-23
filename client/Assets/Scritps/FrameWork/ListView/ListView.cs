using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Linq;

[RequireComponent(typeof(UnityEngine.UI.RectMask2D))]
[RequireComponent(typeof(UnityEngine.UI.LoopScrollRect))]
[DisallowMultipleComponent]
public class ListView : MonoBehaviour, LoopScrollPrefabSource, LoopScrollDataSource
{
    public GameObject Item;
    private int mtotalCount = -1;

    private Action<int, Transform> onItemRender;
    private string panelName;
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
            return Instantiate(Item);
        }
        Transform candidate = pool.Pop();
        candidate.GetComponent<BaseCell>().panelName = panelName;
        candidate.gameObject.SetActive(true);
        return candidate.gameObject;
    }

    public void ReturnObject(Transform trans)
    {
        // Use `DestroyImmediate` here if you don't need Pool
        trans.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
        trans.gameObject.SetActive(false);
        trans.SetParent(transform, false);
        pool.Push(trans);
    }


    public void ProvideData(Transform transform, int idx)
    {
        onItemRender(idx, transform);
    }

    public void SetItemRender(object obj, Action<int, Transform> itemRender)
    {
        panelName = obj.GetType().Name;
        onItemRender = itemRender;
    }
}
