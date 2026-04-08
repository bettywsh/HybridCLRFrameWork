using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringHelper
{
    public static bool CodeValidate(string value)
    {
        if (string.IsNullOrEmpty(value)) return false;
        if (string.IsNullOrWhiteSpace(value) || value.Length is < 6 )
        {
            //OnEnableMessageDisplay(true, "手机号输入有误");
            return false;
        }

        return true;
    }
}
