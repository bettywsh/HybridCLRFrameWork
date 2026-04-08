using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LoopScrollCricle : MonoBehaviour
{
    public float radius = 100f; // 半径
    private List<GameObject> loopItems = new List<GameObject>();
    public List<GameObject> loopButtons = new List<GameObject>();
    private List<float> yOffsets;
    private List<int> sortOrders;
    public float yOffsetStep = 80f; // Y轴偏移步长
    public int levelCount = 6; // 关卡数量
    private float angleStep;
    Coroutine moveCoroutine;
    public float moveDuration = 1f; // 移动持续时间
    private float currentAngle = 0f;
    int parentOrders;
    int moveToIndex = 0;
    public Action<int> OnClickCallBack;
    bool isAni = true;
    void ShowButton(int index)
    {
        for (int i = 0; i < loopButtons.Count; i++)
        {
            if (index == i)
            {
                loopButtons[i].SetActive(false);
            }
            else
            {
                loopButtons[i].SetActive(true);
            }
        }
    }

    void HideButton()
    {
        for (int i = 0; i < loopButtons.Count; i++)
        {
            loopButtons[i].SetActive(false);
        }
    }

    public void SetSortOrders(int orders)
    {
        parentOrders = orders;

        // 计算每个按钮之间的角度
        angleStep = 360f / levelCount;
        // 自动计算对称的Y轴偏移量
        yOffsets = CalculateSymmetricYOffsets(levelCount);

        // 初始化排序顺序
        sortOrders = CalculateRenderingOrder(levelCount);
        for (int i = 0; i < sortOrders.Count; i++)
        {
            sortOrders[i] += orders;
        }

        // 创建关卡按钮并设置它们的位置
        for (int i = 0; i < levelCount; i++)
        {
            loopItems.Add(transform.GetChild(i).gameObject);
        }
        UpdateButtonPositions();
        ShowButton(0);
    }

    public void OnClickItem(int i)
    {
        MoveTo(i, true);
    }

    void UpdateButtonPositions()
    {
        for (int i = 0; i < loopItems.Count; i++)
        {
            float angle = (360f / loopItems.Count) * i;
            float x = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            float z = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;

            // 获取计算后的Y轴偏移
            float yOffset = yOffsets[i];

            // 设置按钮位置
            RectTransform rectTransform = loopItems[i].GetComponent<RectTransform>();
            rectTransform.anchoredPosition3D = new Vector3(x, yOffset, 0);

            // 设置Canvas的sortingOrder
            Canvas canvas = loopItems[i].GetComponent<Canvas>();
            canvas.sortingOrder = sortOrders[i];

            // 设置透明度和缩放值
            SetButtonAppearance(loopButtons[i], sortOrders[i]);
        }

    }

    public void MoveTo(int index, bool isani)
    {
        isAni = isani;
        int bottomIndex = sortOrders.IndexOf(sortOrders.Count / 2 + 1 + parentOrders);
        int currentIndex = int.Parse(loopItems[bottomIndex].name);
        Debug.Log("当前显示 " + currentIndex);
        int clockwise = (index - currentIndex + levelCount) % levelCount;
        if (clockwise == 0) clockwise = levelCount;
        int counterclockwise = levelCount - clockwise;

        if (moveCoroutine != null)
        {
            return;
        }
        moveToIndex = index;
        HideButton();
        if (clockwise < counterclockwise)
            moveCoroutine = StartCoroutine(MoveButtonCoroutine(clockwise, true));
        else if (counterclockwise < clockwise)
            moveCoroutine = StartCoroutine(MoveButtonCoroutine(counterclockwise, false));
        else
            moveCoroutine = StartCoroutine(MoveButtonCoroutine(clockwise, true));

    }

    IEnumerator MoveButtonCoroutine(int movesRequired, bool isCounterclockwise)
    {
        int t = 0;
        int diction = isCounterclockwise ? -1 : 1;
        while (movesRequired > t)
        {
            RotateButtons(diction);
            Debug.Log("移动一次");

            if (isAni)
            {
                yield return new WaitForSeconds(moveDuration);
            }
            t++;
        }
        moveCoroutine = null;
    }

    void RotateButtons(int direction)
    {
        //nextLevelButton.interactable = false;
        //previousLevelButton.interactable = false;

        List<Vector3> targetPositions = new List<Vector3>();
        List<float> targetYOffsets = new List<float>();
        List<int> newSortOrders = new List<int>();

        // 计算目标位置、Y轴偏移和顺序
        for (int i = 0; i < loopItems.Count; i++)
        {
            int targetIndex = (i + direction + levelCount) % levelCount;
            float angle = currentAngle + angleStep * targetIndex;
            float x = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            float z = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float yOffset = yOffsets[targetIndex];

            targetPositions.Add(new Vector3(x, yOffset, 0));
            targetYOffsets.Add(yOffset);
            newSortOrders.Add(sortOrders[targetIndex]);
        }

        // 更新排序顺序
        sortOrders = newSortOrders;
        for (int i = 0; i < loopItems.Count; i++)
        {
            Canvas canvas = loopItems[i].GetComponent<Canvas>();
            canvas.sortingOrder = sortOrders[i];
            // 更新透明度和缩放值
            SetButtonAppearance(loopItems[i], sortOrders[i]);
        }

        // 平滑移动并更新Y轴偏移和排序顺序
        for (int i = 0; i < loopItems.Count; i++)
        {
            RectTransform rectTransform = loopItems[i].GetComponent<RectTransform>();
            int index = i;

            if (isAni)
            {
                // 平滑移动位置
                rectTransform.DOAnchorPos3D(targetPositions[i], moveDuration).OnComplete(() =>
                {
                    // 更新Y轴偏移
                    if (index < yOffsets.Count)
                    {
                        yOffsets[index] = targetYOffsets[index];
                    }
                    if (index == loopItems.Count - 1)
                    {
                        Debug.Log("完成移动");
                        ShowButton(moveToIndex - 1);
                        OnClickCallBack(moveToIndex);
                        // 重新启用按钮
                        //nextLevelButton.interactable = true;
                        //previousLevelButton.interactable = true;

                        // 打印最上层和最下层按钮的名称
                        //PrintTopButtonName();
                        //PrintBottomButtonName();
                    }
                });
            }
            else
            {
                rectTransform.anchoredPosition3D = targetPositions[i];
                if (index < yOffsets.Count)
                {
                    yOffsets[index] = targetYOffsets[index];
                }
                if (index == loopItems.Count - 1)
                {
                    Debug.Log("完成移动");
                    ShowButton(moveToIndex - 1);
                    OnClickCallBack(moveToIndex);
                }
            }
        }

        // 更新角度
        currentAngle += angleStep * direction;
    }

    void SetButtonAppearance(GameObject button, int sortOrder)
    {
        //float alpha = Mathf.Lerp(0.8f, 1f, ((float)sortOrder - parentOrders - 1) / (levelCount / 2));
        float scale = Mathf.Lerp(0.8f, 1f, ((float)sortOrder - parentOrders - 1) / (levelCount / 2));

        // 设置透明度
        //CanvasGroup canvasGroup = button.GetComponent<CanvasGroup>();
        //if (canvasGroup != null)
        //{
        //    canvasGroup.alpha = alpha;
        //}

        // 设置缩放值
        RectTransform rectTransform = button.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.localScale = new Vector3(scale, scale, scale);
        }
    }

    List<float> CalculateSymmetricYOffsets(int count)
    {
        List<float> offsets = new List<float>();
        float step = yOffsetStep;
        int half = count / 2;
        float y = 0;
        for (int i = 0; i < count; i++)
        {
            if (i != 0)
            {
                y = i * step;
                if (i > half)
                {
                    y = (count - i) * step;
                }
            }
            offsets.Add(y);
        }
        return offsets;
    }

    List<int> CalculateRenderingOrder(int count)
    {
        List<int> order = new List<int>();
        int middle = count / 2 + 1;
        int number = 0;
        for (int i = 0; i < count; i++)
        {
            if (i < middle)
            {
                number = middle - i;
            }
            else
            {
                number = order[count - i];
            }
            order.Add(number);
        }
        return order;
    }
}
