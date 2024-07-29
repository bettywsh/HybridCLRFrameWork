using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RedPointConst
{
    public const string Root = "Root";
    public const string Model1 = "Root|Model1";
    public const string Model1_Model1 = "Root|Model1|Model1";
    public const string Model2 = "Root|Model2";
    public const string Model2_Model2 = "Root|Model2|Model2";
}

public class RedManager : Singleton<RedManager>
{
    RedNode root;

    List<string> redPointList = new List<string>
    {
        RedPointConst.Root
    };
    public override async UniTask Init()
    {
        await base.Init();
        root = new RedNode();
        foreach (string s in redPointList)
        {
            InsterNode(s);
        }
    }

    public void InsterNode(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return;
        }
        if (SearchNode(name) != null)
        {
            Debug.Log("你已经插入了该节点" + name);
            return;
        }

        RedNode parent = root;
        string[] nodes = name.Split('|');
        foreach (var node in nodes)
        {
            if (!parent.childs.ContainsKey(node))
            {
                parent.childs.Add(node, new RedNode(node, parent));
            }
            parent = parent.childs[node];
        }
    }

    public RedNode SearchNode(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return null;
        }
        RedNode parent = null;
        string[] nodes = name.Split('|');
        foreach (var node in nodes)
        {
            if (!parent.childs.ContainsKey(node))
            { 
                return null;
            }
            parent = parent.childs[node];
        }
        return parent;
    } 

    public void SetNum(string nodeName, int num)
    {
        var nodeList = nodeName.Split("|");
        if (nodeList.Length == 1)
        {
            if (nodeList[0] != RedPointConst.Root)
            {
                return;
            }
            var node = root;
            for (int i = 0; i < nodeList.Length; i++)
            {
                if (!node.childs.ContainsKey(nodeList[i]))
                {
                    Debug.Log("Does Not Contains Child Node:" + nodeList[i]);
                    return;
                }
                node = node.childs[nodeList[i]];
                if (i == nodeList.Length - 1)
                {
                    node.SetRedPointNum(num);
                }
            }
        }
    }
}

public class RedNode
{
    /// <summary>
    /// 节点名
    /// </summary>
    public string name;

    /// <summary>
    /// 红点数
    /// </summary>
    public int num;

    public RedNode parent;

    public Action updateCb;

    public Dictionary<string, RedNode> childs = new Dictionary<string, RedNode>();

    public RedNode()
    {
        this.name = RedPointConst.Root;
    }
    public RedNode(string name, RedNode parent)
    { 
        this.name = name;
        this.parent = parent;
    }

    public void SetRedPointNum(int num)
    {
        if (childs.Count > 0)
        {
            Debug.Log("Only Can Set Leaf Node!");
            return;
        }
        this.num = num;
        Notify();

        if (parent != null)
        {
            parent.ChangeRedPointNum();
        }
    }

    public void ChangeRedPointNum()
    { 
        int newNum = 0;
        foreach (RedNode node in childs.Values)
        {
            newNum += node.num;
        }
        if (newNum != num)
        {
            num = newNum;
            Notify();
        }
        if (parent != null)
        {
            parent.ChangeRedPointNum();
        }
    }

    public void Notify()
    { 
    
    }
}
