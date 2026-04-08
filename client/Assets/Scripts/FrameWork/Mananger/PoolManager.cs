using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<string, List<GameObject>> poolList = new Dictionary<string, List<GameObject>>();
    private Dictionary<string, Stack<GameObject>> pools = new Dictionary<string, Stack<GameObject>>();

    public void InitPool(string poolName, GameObject poolObjectPrefab, int initCount, Transform poolRoot)
    {        
        //populate the pool
        for (int index = 0; index < initCount; index++)
        {
            GameObject go = NewObjectInstance(poolObjectPrefab);
            go.transform.SetParent(poolRoot, false);
            if (!poolList.TryGetValue(poolName, out var goList))
            {
                goList = new List<GameObject>();
                poolList.Add(poolName, goList);
            }
            goList.Add(go);

            AddObjectToPool(poolName, go);
        }
    }

    public GameObject CreatePool(string poolName, GameObject poolObjectPrefab, Transform poolRoot)
    {
        GameObject go = NextAvailableObject(poolName);
        if (go == null)
        {
            go = NewObjectInstance(poolObjectPrefab);
            go.transform.SetParent(poolRoot, false);
            if (!poolList.TryGetValue(poolName, out var goList))
            {
                goList = new List<GameObject>();
                poolList.Add(poolName, goList);
            }
            goList.Add(go);

            AddObjectToPool(poolName, go);
            go = NextAvailableObject(poolName);
        }
        return go;
    }

    //o(1)
    private void AddObjectToPool(string poolName, GameObject go)
    {
        if (go == null)
        {
            return;
        }
        //add to pool
        go.SetActive(false);
        if (!pools.TryGetValue(poolName, out var availableObjStack))
        {
            availableObjStack = new Stack<GameObject>();
            pools.Add(poolName, availableObjStack);
        }
        availableObjStack.Push(go);
    }

    private GameObject NewObjectInstance(GameObject go)
    {
        return GameObject.Instantiate(go);
    }

    public GameObject NextAvailableObject(string poolName)
    {
        GameObject go = null;
        if (!pools.TryGetValue(poolName, out var availableObjStack))
            return null;
        if (availableObjStack.Count > 0)
        {
            go = availableObjStack.Pop();
        }
        if (go != null)
        {
            go.SetActive(true);
        }
        return go;
    }

    //o(1)
    public void ReturnObjectToPool(string poolName, GameObject po)
    {
        AddObjectToPool(poolName, po);
    }

    public void DestoryPoolName(string poolName)
    {
        if (poolList.TryGetValue(poolName, out var availableObjStack))
        {
            foreach (var item in availableObjStack)
            {
                GameObject.Destroy(item.gameObject);
            }
            availableObjStack.Clear();
        }
    }

    public void DestoryPoolAll()
    {
        foreach (var pool in poolList)
        {
            DestoryPoolName(pool.Key);
        }
    }
}