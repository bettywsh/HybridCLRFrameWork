using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateTimeHelper : MonoBehaviour
{
    /// <summary>
    /// 天数差 按0点计算
    /// </summary>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <returns></returns>
    public static int DateTimeSubtract(long startTime, long endTime)
    {
        DateTime startDateTime = DateTimeOffset.FromUnixTimeMilliseconds(startTime).UtcDateTime.AddHours(8);
        DateTime endDateTime = DateTimeOffset.FromUnixTimeMilliseconds(endTime).UtcDateTime.AddHours(8);
        return (int)(endDateTime.Date - startDateTime.Date).TotalDays;
    }

    /// <summary>
    /// 到0点的秒数
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public static int ToDayRemainingSeconds(long time)
    {
        DateTime startDateTime = DateTimeOffset.FromUnixTimeMilliseconds(time).UtcDateTime.AddHours(8);
        DateTime endDateTime = DateTimeOffset.FromUnixTimeMilliseconds(time).UtcDateTime.AddHours(8).Date.AddDays(1);
        return (int)(endDateTime - startDateTime).TotalSeconds;
    }

    /// <summary>
    /// 判断是否是一天
    /// </summary>
    /// <param name="timestamp1"></param>
    /// <param name="timestamp2"></param>
    /// <returns></returns>
    public static bool ToDay(long timestamp1, long timestamp2)
    {
        // 将时间戳转换为DateTime对象（UTC）  
        DateTime dateTime1 = DateTimeOffset.FromUnixTimeMilliseconds(timestamp1).UtcDateTime.AddHours(8);
        DateTime dateTime2 = DateTimeOffset.FromUnixTimeMilliseconds(timestamp2).UtcDateTime.AddHours(8);

        // 比较年份、月份和日份  
        return dateTime1.Year == dateTime2.Year &&
               dateTime1.Month == dateTime2.Month &&
               dateTime1.Day == dateTime2.Day;
    }

    public static bool ToDay(int timestamp1, int timestamp2)
    {
        // 将时间戳转换为DateTime对象（UTC）  
        DateTime dateTime1 = DateTimeOffset.FromUnixTimeSeconds(timestamp1).UtcDateTime.AddHours(8);
        DateTime dateTime2 = DateTimeOffset.FromUnixTimeSeconds(timestamp2).UtcDateTime.AddHours(8);

        // 比较年份、月份和日份  
        return dateTime1.Year == dateTime2.Year &&
               dateTime1.Month == dateTime2.Month &&
               dateTime1.Day == dateTime2.Day;
    }

    public static DateTime ToDateTime(long timestamp1)
    {
        DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp1).UtcDateTime.AddHours(8);
        return dateTime;
    }

    public static long ToBeiJing(long timestamp1)
    {
        DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp1).UtcDateTime.AddHours(8);
        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return (dateTime.Ticks - unixEpoch.Ticks) / 10000;
    }

    /// <summary>
    /// 下一个周三零点
    /// </summary>
    /// <param name="timestamp1"></param>
    /// <returns></returns>
    public static long NextWednesday(long timestamp1)
    {
        DateTime today = DateTimeOffset.FromUnixTimeMilliseconds(timestamp1).UtcDateTime.Date;
        DayOfWeek currentDay = today.DayOfWeek;
        int daysUntilWednesday = ((int)DayOfWeek.Wednesday - (int)currentDay + 7) % 7;

        // 如果今天已经是周三，则计算下周三（+7天）
        if (daysUntilWednesday == 0)
        {
            daysUntilWednesday = 7;
        }
        DateTime nextWednesday = today.AddDays(daysUntilWednesday);
        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        long t = (nextWednesday.ToUniversalTime().Ticks - unixEpoch.Ticks) / 10000;
        return t;
    }
}
