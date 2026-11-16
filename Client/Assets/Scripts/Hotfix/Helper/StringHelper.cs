using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringHelper
{
    /// <summary>
    /// 数字转化成带“万”“亿”的字符串
    /// </summary>
    /// <param name="num">要转化的数字</param>
    /// <param name="keepDecimal">是否保留两位小数</param>
    /// <returns>如：12345678 -> "1234.57万"或"1234万"</returns>
    public static string FormatNumberToWanYi(long num, bool keepDecimal = false)
    {
        if (num >= 100000000)
        {
            double value = num / 100000000.0;
            if (keepDecimal)
                return value.ToString("0.00") + "亿";
            else
                return ((long)value).ToString() + "亿";
        }
        else if (num >= 10000)
        {
            double value = num / 10000.0;
            if (keepDecimal)
                return value.ToString("0.00") + "万";
            else
                return ((long)value).ToString() + "万";
        }
        else
        {
            return num.ToString();
        }
    }
}
