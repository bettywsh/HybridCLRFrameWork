using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RedManager : Singleton<RedManager>
{
    RedNode root;


    public override void Init()
    {
        root = new RedNode(RedPointConst.Root);

        // 通过反射获取 RedPointConst 类中的所有 public static 字符串常量
        var type = typeof(RedPointConst);
        foreach (var field in type.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
        {
            if (field.FieldType == typeof(string))
            {
                var value = field.GetValue(null) as string;
                if (!string.IsNullOrEmpty(value))
                    InsterNode(value);
            }
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
            return;
        }

        RedNode parent = root;
        string[] nodes = name.Replace("Root|", "").Split(new char[] { '|' });
        foreach (var node in nodes)
        {
            if (!parent.childs.ContainsKey(node))
            {
                parent.childs.Add(node, new RedNode(name, parent));
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
        if (name == RedPointConst.Root)
        {
            return root;
        }
        RedNode parent = root;
        string[] nodes = name.Replace("Root|", "").Split(new char[] { '|' });
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

    public void SetNum(string nodeName, bool isPoint)
    {
        var nodeList = nodeName.Split(new char[] { '|' });
        if (nodeList.Length >= 1)
        {
            if (nodeList[0] != RedPointConst.Root)
            {
                return;
            }
            var node = root;
            for (int i = 1; i < nodeList.Length; i++)
            {
                if (!node.childs.ContainsKey(nodeList[i]))
                {
                    Debug.Log("Does Not Contains Child Node:" + nodeList[i]);
                    return;
                }
                node = node.childs[nodeList[i]];
                if (i == nodeList.Length - 1)
                {
                    node.SetRedPointNum(isPoint ? 1 : 0);                   
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

    ///// <summary>
    ///// 节点路径
    ///// </summary>
    //public string namePath;

    /// <summary>
    /// 红点数
    /// </summary>
    public int num;

    /// <summary>
    /// 是否有红点
    /// </summary>
    public bool isPoint => num == 0 ? false : true;

    public RedNode parent;

    public Action updateCb;

    public Dictionary<string, RedNode> childs = new Dictionary<string, RedNode>();

    public RedNode(string name)
    {
        this.name = name;
        //this.namePath = name;
        this.num = 0;
        this.parent = null;
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
        Notify(name);

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
            Notify(name);
        }
        if (parent != null)
        {
            parent.ChangeRedPointNum(); 
        }
    }

    public void Notify(string name)
    {
        EventManager.Instance.MessageNotify(MessageConst.Msg_RedPointRefresh, name);
    }
}
