using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

public static class MethodInfoHelper
{
    /// <summary>
    /// 将 MethodInfo 转换为 EventDelegate。
    /// 对于实例方法，必须传入目标对象 target。
    /// </summary>
    public static EventDelegate ToEventDelegate(this MethodInfo method, object target = null)
    {
        if (method == null) throw new ArgumentNullException(nameof(method));
        if (!method.IsStatic && target == null)
            throw new ArgumentException("实例方法必须提供 target 对象");

        ParameterInfo[] parameters = method.GetParameters();
        ParameterExpression argsParam = Expression.Parameter(typeof(object[]), "args");

        // 构造参数表达式数组：从 object[] 中取出并转换为目标类型
        Expression[] argExprs = new Expression[parameters.Length];
        for (int i = 0; i < parameters.Length; i++)
        {
            ConstantExpression indexExpr = Expression.Constant(i);
            Expression arrayAccess = Expression.ArrayIndex(argsParam, indexExpr);       // args[i]
            Expression convertExpr = Expression.Convert(arrayAccess, parameters[i].ParameterType); // (Type)args[i]
            argExprs[i] = convertExpr;
        }

        // 构建调用表达式
        Expression callExpr;
        if (method.IsStatic)
        {
            callExpr = Expression.Call(method, argExprs);
        }
        else
        {
            ConstantExpression instanceExpr = Expression.Constant(target, method.DeclaringType);
            callExpr = Expression.Call(instanceExpr, method, argExprs);
        }

        // 生成 lambda: (object[] args) => method(...)
        LambdaExpression lambda = Expression.Lambda(typeof(EventDelegate), callExpr, argsParam);
        return (EventDelegate)lambda.Compile();
    }
}
