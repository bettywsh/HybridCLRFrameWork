using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBannerSubPanel : SubPanelBase
{
    public Stack<string> msgs = new Stack<string>();
    private bool isAnim = false;
    public override void OnBindEvent()
    {
        base.OnBindEvent();
    }
    public override async UniTask OnOpen()
    {
        await base.OnOpen();
        AddMsg("你好！亲爱的祖国！！！你好！亲爱的祖国！！！你好！亲爱的祖国！！！");
        AddMsg("你好！亲爱的祖国！！！你好！亲爱的祖国！！！你好！亲爱的祖国！！！");
        AddMsg("你好！亲爱的祖国！！！你好！亲爱的祖国！！！你好！亲爱的祖国！！！");
        AddMsg("你好！亲爱的祖国！！！你好！亲爱的祖国！！！你好！亲爱的祖国！！！");
        AddMsg("你好！亲爱的祖国！！！你好！亲爱的祖国！！！你好！亲爱的祖国！！！");
        AddMsg("你好！亲爱的祖国！！！你好！亲爱的祖国！！！你好！亲爱的祖国！！！");
    }

    public void AddMsg(string msg) {
        msgs.Push(msg);
        if (!isAnim)
        {
            isAnim = true;
            GetUI("MainPanel").goValue.SetActive(true);
            MsgShowAnim();
        }
    }

    private void MsgShowAnim()
    {
        float x = 0;
        float time = 0;
        float maxTime = 0;
        string msg = msgs.Pop();
        GetUI("txtMsg").tmptxtValue.text = msg;
        GetUI("txtMsg").tranValue.localPosition = new Vector3(0, -35, 0);
        float txtWidth = GetUI("txtMsg").tmptxtValue.preferredWidth;
        x = (txtWidth - 920) * 0.5f;
        if (x > 0)
        {
            if (x < 40)
            {
                x = 40;
            }
            maxTime = 1.5f;
            time = x / 50;
            if (time > maxTime)
            {
                time = maxTime;
            }
        }
        else
        {
            x = 0;
        }
        Sequence seq = DOTween.Sequence();
        seq.Append(GetUI("txtMsg").tranValue.DOLocalMoveY(0, 0.5f));
        seq.Append(GetUI("txtMsg").tranValue.DOLocalMoveX(x * - 1, time).SetEase(DG.Tweening.Ease.Linear));
        seq.AppendInterval(1);
        seq.Append(GetUI("txtMsg").tranValue.DOLocalMoveY(40, 0.5f));
        seq.AppendCallback(() =>{
            if (msgs.Count > 0)
            {
                MsgShowAnim();
            }
            else
            {
                isAnim = false;
                GetUI("MainPanel").goValue.SetActive(false);
            }
        });
    }

    public override void OnUnBindEvent()
    {
        base.OnUnBindEvent();
    }

    public override void OnClose()
    {
        cancellationTokenSource.Cancel();
    }
}
