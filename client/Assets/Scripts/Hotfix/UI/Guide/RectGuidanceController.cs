using cfg;
using Cysharp.Threading.Tasks;
using Msg;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RectGuidanceController : MonoBehaviour, IPointerClickHandler
{

    public bool isButton;

    public Action clickOnComplete;

    public Action animationOnComplete;
    //获取画布
    private Canvas canvas;

    /// <summary>
    /// 高亮显示的目标
    /// </summary>
    public RectTransform target;

    public RectTransform boder;

    /// <summary>
    /// 区域范围缓存
    /// </summary>
    private Vector3[] _corners = new Vector3[4];

    /// <summary>
    /// 镂空区域中心
    /// </summary>
    private Vector4 _center;

    private Vector3 centerWorld;

    /// <summary>
    /// 最终的偏移值X
    /// </summary>
    private float _targetOffsetX = 0f;

    /// <summary>
    /// 最终的偏移值Y
    /// </summary>
    private float _targetOffsetY = 0f;

    /// <summary>
    /// 遮罩材质
    /// </summary>
    private Material _material;

    /// <summary>
    /// 当前的偏移值X
    /// </summary>
    private float _currentOffsetX = 0f;

    /// <summary>
    /// 当前的偏移值Y
    /// </summary>
    private float _currentOffsetY = 0f;

    /// <summary>
    /// 动画收缩时间
    /// </summary>
    private float _shrinkTime = 0.2f;


    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    /// <summary>
    /// 世界坐标到画布坐标的转换
    /// </summary>
    /// <param name="canvas">画布</param>
    /// <param name="world">世界坐标</param>
    /// <returns>转换后在画布的坐标</returns>
    private Vector2 WorldToCanvasPos(Canvas canvas, Vector3 world)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, world,
            canvas.GetComponent<Camera>(), out position);
        return position;
    }

    private Vector3 WorldToScreenPoint()
    {
        return UIManager.Instance.uiCamera.WorldToScreenPoint(centerWorld);
    }

   

    public async void SetTarget(Transform target)
    {
        this.target = target as RectTransform;
        _material = GetComponent<Image>().material;
        _material.SetFloat("_SliderX", 0);
        _material.SetFloat("_SliderY", 0);
        bool isCanel = await UniTask.WaitForSeconds(0.5f, true, cancellationToken: cancellationTokenSource.Token).SuppressCancellationThrow();
        if (isCanel)
            return;
        RefreshMask();
    }

    private async void RefreshMask()
    {
        canvas = UIManager.Instance.uiCanvas;
        //获取高亮区域四个顶点的世界坐标
        target.GetWorldCorners(_corners);

        //计算高亮显示区域咋画布中的范围
        _targetOffsetX = Vector2.Distance(WorldToCanvasPos(canvas, _corners[0]), WorldToCanvasPos(canvas, _corners[3])) / 2f;
        _targetOffsetY = Vector2.Distance(WorldToCanvasPos(canvas, _corners[0]), WorldToCanvasPos(canvas, _corners[1])) / 2f;
        //计算高亮显示区域的中心
        float x = _corners[0].x + ((_corners[3].x - _corners[0].x) / 2f);
        float y = _corners[0].y + ((_corners[1].y - _corners[0].y) / 2f);
        centerWorld = new Vector3(x, y, 0);
        Vector2 center = WorldToCanvasPos(canvas, centerWorld);

        //设置遮罩材料中中心变量
        _center = new Vector4(center.x, center.y, 0, 0);
        _material = GetComponent<Image>().material;
        _material.SetVector("_Center", _center);
        //计算当前偏移的初始值
        RectTransform canvasRectTransform = (canvas.transform as RectTransform);
        if (canvasRectTransform != null)
        {
            //获取画布区域的四个顶点
            canvasRectTransform.GetWorldCorners(_corners);
            //求偏移初始值
            for (int i = 0; i < _corners.Length; i++)
            {
                if (i % 2 == 0)
                    _currentOffsetX = Mathf.Max(Vector3.Distance(WorldToCanvasPos(canvas, _corners[i]), center), _currentOffsetX);
                else
                    _currentOffsetY = Mathf.Max(Vector3.Distance(WorldToCanvasPos(canvas, _corners[i]), center), _currentOffsetY);
            }
        }
        //设置遮罩材质中当前偏移的变量
        _material.SetFloat("_SliderX", _currentOffsetX);
        _material.SetFloat("_SliderY", _currentOffsetY);
        //Invoke("AnimationComplete", _shrinkTime + 0.5f);
        bool isCanel = await UniTask.WaitForSeconds(_shrinkTime + 0.5f, true, cancellationToken: cancellationTokenSource.Token).SuppressCancellationThrow();
        if (isCanel)
            return;
        AnimationComplete();
    }

    public void AnimationComplete()
    {
        boder.sizeDelta = new Vector2(target.sizeDelta.x + 55, target.sizeDelta.y + 55);
        boder.gameObject.SetActive(true);
        if (animationOnComplete != null)
            animationOnComplete();
    }

    private float _shrinkVelocityX = 0f;
    private float _shrinkVelocityY = 0f;

    private void Update()
    {
        //从当前偏移值到目标偏移值差值显示收缩动画
        float valueX = Mathf.SmoothDamp(_currentOffsetX, _targetOffsetX, ref _shrinkVelocityX, _shrinkTime, float.PositiveInfinity, Time.unscaledDeltaTime);
        float valueY = Mathf.SmoothDamp(_currentOffsetY, _targetOffsetY, ref _shrinkVelocityY, _shrinkTime, float.PositiveInfinity, Time.unscaledDeltaTime);

        if (!Mathf.Approximately(valueX, _currentOffsetX))
        {
            _currentOffsetX = valueX;
            _material.SetFloat("_SliderX", _currentOffsetX);
        }

        if (!Mathf.Approximately(valueY, _currentOffsetY))
        {
            _currentOffsetY = valueY;
            _material.SetFloat("_SliderY", _currentOffsetY);
        }
    }

    public bool IsRect(Vector2 sp)
    {
        Vector3 center = WorldToScreenPoint();
        bool result = RectTransformUtility.RectangleContainsScreenPoint(target, sp, UIManager.Instance.uiCamera);
        return result;
        //if (center.x - _targetOffsetX < sp.x && center.x + _targetOffsetX > sp.x &&
        //    center.y - _targetOffsetY < sp.y && center.y + _targetOffsetY > sp.y)
        //{
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
    }

    //监听点击
    public void OnPointerClick(PointerEventData eventData)
    {   
        if (IsRect(eventData.position))
        {
            if (clickOnComplete != null)
                clickOnComplete();
            //MessageMgr.Instance.MessageNotify(MessageConst.MsgGuideClickComplete);
            if (isButton)
            {
                //rectGuidanceController.target.GetComponent<Button>().onClick.Invoke();
                Button button = target.GetComponent<Button>();
                if (button != null)
                {
                    button.OnPointerClick(eventData);
                }
                else
                {
                    Toggle toggle = target.GetComponent<Toggle>();
                    if (toggle != null)
                    {
                        toggle.OnPointerClick(eventData);
                    }
                }
            }
            else
            {
                PassEvent(eventData, ExecuteEvents.submitHandler);
                PassEvent(eventData, ExecuteEvents.pointerClickHandler);
                //PassEvent(eventData, ExecuteEvents.c);
                Button button = target.GetComponent<Button>();
                if (button != null)
                {
                    button.OnPointerClick(eventData);
                }
             
            }

          
        }

        //PassEvent(eventData, ExecuteEvents.pointerClickHandler);
    }
    //监听按下
    public void OnPointerDown(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.pointerDownHandler);
    }
    //监听抬起
    public void OnPointerUp(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.pointerUpHandler);
    }

    //把事件透下去
    public void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function)
        where T : IEventSystemHandler
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);
        GameObject current = data.pointerCurrentRaycast.gameObject;
        for (int i = 0; i < results.Count; i++)
        {
            if (current != results[i].gameObject)
            {
                ExecuteEvents.Execute(results[i].gameObject, data, function);
                break;
                //RaycastAll后ugui会自己排序，如果你只想响应透下去的最近的一个响应，这里ExecuteEvents.Execute后直接break就行。
            }
        }
    }

    private void OnDestroy()
    {
        cancellationTokenSource.Cancel();
    }
}