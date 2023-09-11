using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFlyingPanel : BasePanel
{
	public TextFlyingPanelView view;
    bool isTween = false;
    Queue<string> queue = new Queue<string>();

    public override void OnOpen()
    {		
        view = transform.GetComponent<TextFlyingPanelView>();
		base.OnOpen();
    }

    public override void OnUpdate()
     {
        if (!isTween && queue.Count > 0)
        {
            isTween = true;
            view.txt_TextMesh.text = queue.Dequeue();
            Transform run = ObjectHelper.AddChildren(view.obj_Content.transform, view.img_Bg.gameObject);
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
