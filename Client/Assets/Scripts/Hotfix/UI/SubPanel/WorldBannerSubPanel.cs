using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldBannerSubPanel : SubPanelBase
{
    public bool isScrolling = false;
    private float m_speed = 80f;
    public override void OnBindEvent()
    {
        base.OnBindEvent();
    }
    public override async UniTask OnOpen()
    { 
        await base.OnOpen();
        transform.gameObject.SetActive(false);
    }
    [OnMessage(MessageConst.Msg_ShowWorldBanner)]
    public void Scroll()
    {
        if (isScrolling)
            return;
        string content = DataManager.Instance.GetData<WorldBannerData>().GetMsg();
        Vector3 pos = GetUI("txtMsg").tranValue.GetComponent<RectTransform>().localPosition;
        GetUI("txtMsg").tranValue.localPosition = new Vector3(0, pos.y, 0);
        isScrolling = true;
        transform.gameObject.SetActive(true);
        GetUI("txtMsg").tmptxtValue.text = content;
        float txtWidth = GetUI("txtMsg").tmptxtValue.preferredWidth;//文本自身的长度.
        float distance = txtWidth / 2 + 476;
        float duration = distance / m_speed;
        Vector3 startPos = new Vector3(txtWidth / 2 + 476, pos.y, pos.z);
        GetUI("txtMsg").tranValue.localPosition = startPos;
        GetUI("txtMsg").tranValue.GetComponent<RectTransform>().DOLocalMoveX(-distance, duration)
        .SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => {
            transform.gameObject.SetActive(false);
            isScrolling = false;
        }).SetUpdate(true);
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
