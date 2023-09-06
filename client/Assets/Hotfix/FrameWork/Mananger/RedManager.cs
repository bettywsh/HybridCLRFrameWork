using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedManager : Singleton<RedManager>
{
    RedNode root;

    public override void Init()
    {
        //root = CreateNode("root");

        //for k, v in pairs(self.NodeName) do
        //            self.InsertNode(v)
        //end
    }

    RedNode CreateNode(string name)
    {
        RedNode redNode = new RedNode();
        redNode.name = name;
        redNode.passCount = 0;
        redNode.endCount = 0;
        redNode.redPointCount = 0;
        redNode.chilren = new List<RedNode>();
        redNode.updateCb = null;
        return redNode;
    }

    void InsertNode(string name)
    {
        //if (name == "")
        //    return;
        //if (SearchNode(name))
        //    return;
        //RedNode redNode = root;
        //redNode.passCount = redNode.passCount + 1;
        //string[] pathList = name.Split("|");
        //for (int i = 0; i < pathList.Length; i++)
        //{ 
        //    redNode.chilren
        //}
    }
}

public class RedNode
{
    public string name;
    public int passCount;
    public int endCount;
    public int redPointCount;
    public List<RedNode> chilren;
    public Action updateCb;
}
