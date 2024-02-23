using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RedPointConst
{
    public const string Main = "Main";
    public const string Model1 = "Root|Model1";
    public const string Model1_Model1 = "Root|Model1|Model1";
    public const string Model2 = "Root|Model2";
    public const string Model2_Model2 = "Root|Model2|Model2";
}

public class RedManager : Singleton<RedManager>
{
    RedNode main;

    List<string> redPointList = new List<string>
    {
        RedPointConst.Main
    };
    public override void Init()
    {
        main = new RedNode();
        main.name = RedPointConst.Main;
        foreach (string s in redPointList)
        {
            var node = main;
            var treeNodes = s.Split("|");
            if (treeNodes[0] != main.name)
            {
                continue;
            }
            if (treeNodes.Length > 1)
            {
                for (int i = 0; i < treeNodes.Length; i++)
                {
                    if (!node.childs.ContainsKey(treeNodes[i]))
                    {
                        node.childs.Add(treeNodes[i], new RedNode());
                    }
                    node.childs[treeNodes[i]].name = treeNodes[i];
                    node.childs[treeNodes[i]].parent = node;
                    node = node.childs[treeNodes[i]];
                }
            }
        }
    }


    public void SetNum(string nodeName, int num)
    {
        var nodeList = nodeName.Split("|");
        if (nodeList.Length == 1)
        {
            if (nodeList[0] != RedPointConst.Main)
            {
                return;
            }
            var node = main;
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
    public string name;
    public int num;
    public RedNode parent;    
    public Action updateCb;
    public Dictionary<string, RedNode> childs = new Dictionary<string, RedNode>();

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
