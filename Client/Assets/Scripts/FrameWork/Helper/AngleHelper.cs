using UnityEngine;

public class AngleHelper
{
    /// <summary>
    /// 计算两点之间的2D角度（-180 到 180）
    /// </summary>
    public static float AngleBetweenPoints(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(b.y - a.y, b.x - a.x) * Mathf.Rad2Deg;
    }
    
    /// <summary>
    /// 计算两点之间的2D角度（0 到 360）
    /// </summary>
    public static float AngleBetweenPoints360(Vector2 a, Vector2 b)
    {
        var angle = AngleBetweenPoints(a, b);
        return angle < 0 ? angle + 360f : angle;
    }
    
    /// <summary>
    /// 计算两点之间的3D角度（在指定平面上）
    /// </summary>
    public static float AngleBetweenPointsOnPlane(Vector3 a, Vector3 b, Vector3 planeNormal)
    {
        var direction = b - a;
        var projected = Vector3.ProjectOnPlane(direction, planeNormal);
        
        if (projected == Vector3.zero)
            return 0f;
            
        var angle = Vector3.SignedAngle(Vector3.right, projected, planeNormal);
        return angle;
    }
    
    /// <summary>
    /// 将角度标准化到 -180 到 180 范围
    /// </summary>
    public static float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f)
            angle -= 360f;
        else if (angle < -180f)
            angle += 360f;
        return angle;
    }
    
    /// <summary>
    /// 将角度标准化到 0 到 360 范围
    /// </summary>
    public static float NormalizeAngle360(float angle)
    {
        angle %= 360f;
        if (angle < 0f)
            angle += 360f;
        return angle;
    }
}