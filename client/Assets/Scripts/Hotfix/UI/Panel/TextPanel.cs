using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPanel : BasePanel
{
    bool isTween = false;
    Queue<string> queue = new Queue<string>();

    public override async UniTask OnOpen()
    { 
		await base.OnOpen();
        transform.GetComponent<Canvas>().sortingOrder = (int)EUILayer.Text;
    }

    public override void OnUpdate()
     {
        if (!isTween && queue.Count > 0)
        {
            isTween = true;
            referenceData["txtTextMesh"].tmptxtValue.text = queue.Dequeue();
            Transform run = GameObjectHelper.Instantiate(referenceData["objContent"].tranValue, referenceData["imgBg"].tranValue.gameObject);
            run.gameObject.SetActive(true);
            Sequence seq = DOTween.Sequence();
            seq.Append(run.GetComponent<RectTransform>().DOAnchorPosY(125, 1).SetRelative());
            seq.Append(run.GetComponent<CanvasGroup>().DOFade(0, 1));
            seq.AppendCallback(() =>
            {
                isTween = false;
                GameObject.Destroy(run.gameObject);
            });
        }
     }

    public void Fly(string value)
    {    
        queue.Enqueue(value);
    }

    public override void OnClose()
    {
        base.OnClose();
    }
}
