using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RedPointConst
{
    public const string Root = "Root";
    public const string Vip = "Root|Vip";
    public const string Equipment = "Root|Equipment";
    public const string Equipment_Head = "Root|Equipment|Head";
    public const string Equipment_Clothes = "Root|Equipment|Clothes";
    public const string Equipment_Hand = "Root|Equipment|Hand";
    public const string Equipment_Shoes = "Root|Equipment|Shoes";
    public const string Equipment_Belt = "Root|Equipment|Belt";
    public const string Equipment_Weapon = "Root|Equipment|Weapon";
    public const string Battle = "Root|Battle";
    public const string Battle_Email = "Root|Battle|Email";
    public const string Battle_FirstGift = "Root|Battle|FirstGift";
    public const string Battle_SevenDay = "Root|Battle|SevenDay";
    public const string Battle_SuperGift_PerGiftPack = "Root|Battle|SuperGift|PerGiftPack";
    public const string Battle_Charge = "Root|Battle|Charge";
    public const string Battle_Charge_AccumulatedCharge = "Root|Battle|Charge|AccumulatedCharge";
    public const string Battle_Charge_Continuous = "Root|Battle|Charge|Continuous";
    public const string Battle_FreeActivty = "Root|Battle|FreeActivty";
    public const string Battle_FreeActivty_SignIn = "Root|Battle|FreeActivty|SignIn";
    public const string Battle_FreeActivty_Turntable = "Root|Battle|FreeActivty|Turntable";
    public const string Battle_Activty_VipGift = "Root|Battle|FreeActivty|VipGift";
    //public const string Battle_Activty_EquipGift = "Root|Battle|FreeActivty|EquipGift";
    public const string Battle_Activty_BossReward = "Root|Battle|FreeActivty|BossReward";
    public const string Battle_MonthCard = "Root|Battle|MonthCard";
    public const string Battle_SuperGift = "Root|Battle|SuperGift";
    public const string Battle_SuperGift_HighlyProfitable = "Root|Battle|SuperGift|HighlyProfitable";
    public const string Battle_SuperGift_HighlyProfitable_One = "Root|Battle|SuperGift|HighlyProfitable|One";
    public const string Battle_SuperGift_HighlyProfitable_Two = "Root|Battle|SuperGift|HighlyProfitable|Two";
    public const string Battle_SuperGift_HighlyProfitable_Three = "Root|Battle|SuperGift|HighlyProfitable|Three";
    public const string Battle_SuperGift_HighlyProfitable_Four = "Root|Battle|SuperGift|HighlyProfitable|Four";
    public const string Battle_SuperGift_HighlyProfitable_Five = "Root|Battle|SuperGift|HighlyProfitable|Five";
    public const string Battle_SuperGift_HighlyProfitable_Six = "Root|Battle|SuperGift|HighlyProfitable|Six";
    public const string Battle_SuperGift_HighlyProfitable_Seven = "Root|Battle|SuperGift|HighlyProfitable|Seven";
    public const string Battle_Activty = "Root|Battle|Activty";
    
    public const string Battle_SuperGift_Juhuasuan = "Root|Battle|SuperGift|Juhuasuan";
    public const string Battle_DragonKing = "Root|Battle|DragonKing";
    public const string Battle_LuckyDaySign = "Root|Battle|LuckyDaySign";
    public const string Battle_FreeHero = "Root|Battle|FreeHero";
    public const string Battle_FreeHero_Hero = "Root|Battle|FreeHero|Hero";
    public const string Battle_FreeHero_Task = "Root|Battle|FreeHero|Task";
    public const string Battle_Taks1 = "Root|Battle|Taks1";
    public const string Battle_Taks2 = "Root|Battle|Taks2";
    public const string Battle_Taks3 = "Root|Battle|Taks3";
    public const string Battle_Taks4 = "Root|Battle|Taks4";
    public const string Battle_Taks5 = "Root|Battle|Taks5";
    public const string Battle_Level = "Root|Battle|Level";
    public const string Battle_Level_Task = "Root|Battle|Level|Task";
    public const string Battle_Level_Task_TiaoZhan = "Root|Battle|Level|Task|TiaoZhan";
    public const string Battle_Egg = "Root|Battle|Egg";
    public const string Battle_BountyRank = "Root|Battle|BountyRank";
    public const string Battle_ZeroPay = "Root|Battle|ZeroPay";
    public const string Battle_LuckyBox = "Root|Battle|LuckyBox";
    public const string Battle_DragonFight = "Root|Battle|DragonFight";
    public const string Battle_GreatValueGifts = "Root|Battle|GreatValueGifts";
    public const string Battle_GreatValueGifts_Task = "Root|Battle|GreatValueGifts|Task";
    public const string Battle_GreatValueGifts_Recharge = "Root|Battle|GreatValueGifts|Recharge";
    public const string Battle_DragonVault = "Root|Battle|DragonVault ";
    public const string Battle_Egg_GongXian = "Root|Battle|Egg|GongXian";
    public const string Task = "Root|Task";
    public const string Task_Task1 = "Root|Task|Task1";
    public const string Task_Task2 = "Root|Task|Task2";
    public const string Task_Task3 = "Root|Task|Task3";
    public const string Task_Task4 = "Root|Task|Task4";
    public const string Task_Task5 = "Root|Task|Task5";

}

public class RedManager : Singleton<RedManager>
{
    RedNode root;


    public override async UniTask Init()
    {
        await base.Init();
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
            Debug.Log("你已经插入了该节点" + name);
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
