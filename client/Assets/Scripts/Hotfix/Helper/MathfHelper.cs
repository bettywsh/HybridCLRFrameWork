using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathfHelper
{
    public static float Vector2ToAngle(Vector2 v)
    {
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;

        if (angle < 0)
        {
            angle += 360; // 调整角度为0~360度之间
        }

        return angle;
    }
}
