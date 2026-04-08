using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISafeArea
{
    // <summary>
    /// 在刘海屏机子时，是否打开黑边
    /// </summary>
    public const bool OpenBlackBorder = false;

    //启用2倍安全 则左右2边都会裁剪
    public const bool DoubleSafe = false;

    //安全区
    private static Rect g_SafeArea;

    /// <summary>
    /// 安全区
    /// </summary>
    public static Rect SafeArea => g_SafeArea;

    float DesignScreenWidth_F = 1080f;
    float DesignScreenHeight_F = 1920f;
    RectTransform BaseCanvas;
    RectTransform InputCanvas;
    RectTransform left;
    RectTransform right;
    /// <summary>
    /// 横屏设置时，界面左边离屏幕的距离
    /// </summary>
    public static float SafeAreaLeft => Screen.orientation == ScreenOrientation.LandscapeRight
        ? Screen.width - g_SafeArea.xMax
        : g_SafeArea.x;

    private ScreenOrientation ScreenOrientation = Screen.orientation;

    public void InitSafeArea(Transform inputCanvas, Transform baseCanvas, Transform adaptation)
    {
        InputCanvas = inputCanvas.GetComponent<RectTransform>();
        BaseCanvas = baseCanvas.GetComponent<RectTransform>();
        adaptation.gameObject.SetActive(true);
        left = adaptation.Find("Left").GetComponent<RectTransform>();
        right = adaptation.Find("Right").GetComponent<RectTransform>();
        var safeAreaX = Math.Max(Screen.safeArea.x, Screen.width - Screen.safeArea.xMax);
        var safeAreaY = Math.Max(Screen.safeArea.y, Screen.height - Screen.safeArea.yMax);
#if UNITY_EDITOR

        //safeAreaX = 100;
        //safeAreaY = 116;
#endif

        g_SafeArea = new Rect(
            safeAreaX,
            safeAreaY,
            DesignScreenWidth_F - GetSafeValue(safeAreaX),
            DesignScreenHeight_F - GetSafeValue(safeAreaY));

        InitUISafeArea();
    }

    private float GetSafeValue(float safeValue)
    {
        return DoubleSafe ? safeValue * 2 : safeValue;
    }

    private void InitUISafeArea()
    {
        BaseCanvas.anchoredPosition = new Vector2(SafeArea.x, -SafeArea.y);
        InputCanvas.anchoredPosition = new Vector2(SafeArea.x, -SafeArea.y);
        if (DoubleSafe)
        {
            BaseCanvas.offsetMax = new Vector2(-SafeArea.x, BaseCanvas.offsetMax.y);
            BaseCanvas.offsetMin = new Vector2(BaseCanvas.offsetMin.x, SafeArea.y);
        }
        else
        {
            
            float sRatio = (float)Screen.height / (float)Screen.width;
            //float dRatio = DesignScreenHeight_F / DesignScreenWidth_F;
            float CanvasRealHeight = sRatio * DesignScreenWidth_F;
            if (SafeArea.y > 0)
            {
                left.gameObject.SetActive(true);
                left.offsetMax = new Vector2(0, 0);
                left.offsetMin = new Vector2(0, CanvasRealHeight - SafeArea.y);
            }
            else
            {
                left.gameObject.SetActive(false);
            }
            if (SafeArea.x > 0)
            {
                right.gameObject.SetActive(true);
                right.offsetMax = new Vector2(0, -(CanvasRealHeight - SafeArea.x));
                right.offsetMin = new Vector2(0, 0);
            }
            else
            {
                right.gameObject.SetActive(false);
            }
            //TODO 单边时需要考虑手机是左还是右
            BaseCanvas.offsetMax = new Vector2(0, BaseCanvas.offsetMax.y);
            BaseCanvas.offsetMin = new Vector2(0, BaseCanvas.offsetMin.x);

            InputCanvas.offsetMax = new Vector2(0, InputCanvas.offsetMax.y);
            InputCanvas.offsetMin = new Vector2(0, InputCanvas.offsetMin.x);
            //BaseCanvas.anchorMin = new Vector2(CanvasRealHeight/ Screen.height, 0);

        }
    }
}
