using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPanel : PanelBase
{
    Queue<string> queue = new Queue<string>();
    Sequence seq;

    public override async UniTask OnOpen()
    { 
		await base.OnOpen();
        transform.GetComponent<Canvas>().sortingOrder = (int)EUILayer.Text;
    }

    public override void OnUpdate()
     {
        if (queue.Count > 0)
        {
            GetUI("txtTextMesh").tmptxtValue.text = queue.Dequeue();
            Transform run = GameObjectHelper.Instantiate(GetUI("objContent").tranValue, GetUI("imgBg").tranValue.gameObject);
            run.gameObject.SetActive(true);
            seq = DOTween.Sequence();
            seq.SetUpdate(true);
            seq.Append(run.GetComponent<RectTransform>().DOAnchorPosY(125, 1).SetRelative());
            seq.Append(run.GetComponent<CanvasGroup>().DOFade(0, 1));
            seq.AppendCallback(() =>
            {
                GameObject.Destroy(run.gameObject);
            });
        }
     }

    public void Fly(string value)
    {
        seq?.Kill(true);
        queue.Enqueue(value);
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
