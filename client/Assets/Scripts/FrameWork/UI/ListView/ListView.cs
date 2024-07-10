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
    public Action<int, CellBase> OnItemRender;
    public Action<int> OnIemClick;
    public Stack<Transform> pool = new Stack<Transform>();
    Dictionary<int, CellBase> cells = new Dictionary<int, CellBase>();
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


    public GameObject GetObject(int index)
    {
        if (pool.Count == 0)
        {
            GameObject go = Instantiate(Item);            
            Type type = AssemblyManager.Instance.GetType(EAttribute.Cell, Item.name);
            CellBase baseCell = Activator.CreateInstance(type) as CellBase;
            baseCell.Init(go.transform);
            cells.Add(go.transform.GetHashCode(), baseCell);
            return go;
        }
        Transform candidate = pool.Pop();
        candidate.gameObject.SetActive(true);
        return candidate.gameObject;
    }

    public void ReturnObject(Transform trans)
    {
        // Use `DestroyImmediate` here if you don't need Pool
        trans.gameObject.SetActive(false);
        trans.SetParent(transform, false);
        pool.Push(trans);
    }

    public CellBase Get(int key)
    {
        CellBase cellBase;
        if (!cells.TryGetValue(key, out cellBase))
        {
            return null;
        }
        return cellBase;
    }


    public void ProvideData(Transform transform, int idx)
    {
        OnItemRender?.Invoke(idx, Get(transform.GetHashCode()));
    }


    private void OnDestroy()
    {
        foreach ((int k, CellBase v) in cells)
        { 
            v.Dispose();
        }
        cells = null;
    }
}
