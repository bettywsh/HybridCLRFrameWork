using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class EditorTools
{
    public static void FocusUnityGameWindow()
    {
        System.Type T = Assembly.Load("UnityEditor").GetType("UnityEditor.GameView");
        EditorWindow.GetWindow(T, false, "GameView", true);
    }


    public static void SaveCurrentShaderVariantCollection(string savePath)
    {
        EditorTools.InvokeNonPublicStaticMethod(typeof(ShaderUtil), "SaveCurrentShaderVariantCollection", savePath);
    }

    public static void ClearCurrentShaderVariantCollection()
    {
        EditorTools.InvokeNonPublicStaticMethod(typeof(ShaderUtil), "ClearCurrentShaderVariantCollection");
    }

    /// <summary>
    /// ����˽�еľ�̬����
    /// </summary>
    /// <param name="type">�������</param>
    /// <param name="method">����Ҫ���õķ�����</param>
    /// <param name="parameters">���÷�������Ĳ���</param>
    public static object InvokeNonPublicStaticMethod(System.Type type, string method, params object[] parameters)
    {
        var methodInfo = type.GetMethod(method, BindingFlags.NonPublic | BindingFlags.Static);
        if (methodInfo == null)
        {
            UnityEngine.Debug.LogError($"{type.FullName} not found method : {method}");
            return null;
        }
        return methodInfo.Invoke(null, parameters);
    }


    /// <summary>
    /// ��ʾ���ȿ�
    /// </summary>
    public static void DisplayProgressBar(string tips, int progressValue, int totalValue)
    {
        EditorUtility.DisplayProgressBar("����", $"{tips} : {progressValue}/{totalValue}", (float)progressValue / totalValue);
    }

    /// <summary>
    /// ���ؽ��ȿ�
    /// </summary>
    public static void ClearProgressBar()
    {
        EditorUtility.ClearProgressBar();
    }
}
