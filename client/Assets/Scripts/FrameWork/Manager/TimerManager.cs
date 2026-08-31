using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.SocialPlatforms;

public class TimerManager : MonoSingleton<TimerManager>
{
    public override void Init()
    {

    }


    #region 延时定时器

    public void OnceTimer(int timerId, double time, bool timeScale = true)
    {
        if (time < 0.1f)
        {
            Debug.LogError("定时器结束时间太小");
            return;
        }
        if (timerInfos.TryGetValue(timerId, out TimerInfo timerInfo))
        {
            Debug.LogError("已经有相同ID的定时器");
            return;
        }
        long t = (long)(time * 1000);
        TimerInfo timer = new(GetNow(timeScale), GetNow(timeScale) + t, t, TimerType.OnceTimer, timeScale);
        AddTimer(timerId, ref timer);
    }
    #endregion

    #region 倒计时定时器
    public void RepeatedTimer(int timerId, double time, float interval, bool timeScale = true)
    {
        if (time < 0.1f)
        {
            Debug.LogError("定时器结束时间太小");
            return;
        }
        if (timerInfos.TryGetValue(timerId, out TimerInfo timerInfo))
        {
            Debug.LogError("已经有相同ID的定时器"+ timerId);
            return;
        }
        int i = (int)(interval * 1000);
        long t = (long)(time * 1000);
        TimerInfo timer = new(GetNow(timeScale), GetNow(timeScale) + t, i, TimerType.RepeatedTimer, timeScale);
        AddTimer(timerId, ref timer);
    }
    #endregion

    #region 定时器逻辑
    public NativeCollection.MultiMap<long, int> timeId = new(1000);
    public NativeCollection.MultiMap<long, int> timeIdUnscaled = new(1000);
    public Dictionary<long, TimerInfo> timerInfos = new();
    public Queue<long> timeOutTime = new();
    public Queue<long> timeOutTimes = new();

    public long minTime = long.MaxValue;
    public long minTimeUnscaled = long.MaxValue;

    public void Clear(long id)
    {
        if (!timerInfos.TryGetValue(id, out TimerInfo timer))
            return;

        long allTime = timer.StartTime + timer.Interval;
        if (timer.TimeScale)
        {
            timeId.Remove(allTime, (int)id);
            minTime = NextMinTime(timeId, true);
        }
        else
        {
            timeIdUnscaled.Remove(allTime, (int)id);
            minTimeUnscaled = NextMinTime(timeIdUnscaled, false);
        }
        Remove(id);
    }

    private long NextMinTime(NativeCollection.MultiMap<long, int> map, bool timeScale)
    {
        if (map.Count == 0)
            return long.MaxValue;

        long now = GetNow(timeScale);
        long first = long.MaxValue;
        foreach (var kv in map)
        {
            long k = kv.Key;
            if (first == long.MaxValue)
                first = k;
            if (k > now)
                return k;
        }
        return first;
    }

    public bool Remove(long id)
    {
        if (id == 0)
        {
            return false;
        }

        if (!timerInfos.Remove(id))
        {
            return false;
        }
        return true;
    }

    private long GetNow(bool timeScale)
    {
        if (timeScale)
        {
            return TimeManager.Instance.TimerDeltaTime;
        }
        else 
        {
            return TimeManager.Instance.TimerUnscaledDeltaTime;
        }
    }

    private void AddTimer(int timerId, ref TimerInfo timer)
    {
        long tillTime = timer.StartTime + timer.Interval;

        timerInfos.Add(timerId, timer);
        if (timer.TimeScale)
        {
            timeId.Add(tillTime, timerId);
            if (tillTime < minTime)
            {
                minTime = tillTime;
            }
        }
        else
        {
            timeIdUnscaled.Add(tillTime, timerId);
            if (tillTime < minTimeUnscaled)
            {
                minTimeUnscaled = tillTime;
            }
        }
    
    }

    public void Update()
    {
        if (timeId.Count > 0)
        {
            long timeNow = GetNow(true);
            if (timeNow >= minTime)
            {
                TimeOut(timeId, ref minTime, timeNow);
            }
        }
        if (timeIdUnscaled.Count > 0)
        {
            long timeNow = GetNow(false);
            if (timeNow >= minTimeUnscaled)
            {
                TimeOut(timeIdUnscaled, ref minTimeUnscaled, timeNow);
            }
        }
    }

    private void TimeOut(NativeCollection.MultiMap<long, int> map, ref long minTimeRef, long timeNow)
    {
        foreach (var kv in map)
        {
            long k = kv.Key;
            if (k > timeNow)
            {
                minTimeRef = k;
                break;
            }

            timeOutTime.Enqueue(k);
        }

        while (timeOutTime.Count > 0)
        {
            long time = timeOutTime.Dequeue();
            var list = map[time];
            for (int i = 0; i < list.Length; ++i)
            {
                timeOutTimes.Enqueue(list[i]);
            }
            map.Remove(time);
        }

        if (map.Count == 0)
        {
            minTimeRef = long.MaxValue;
        }

        while (timeOutTimes.Count > 0)
        {
            long timerId = timeOutTimes.Dequeue();
            if (!timerInfos.Remove(timerId, out TimerInfo timerInfo))
            {
                continue;
            }
            Run((int)timerId, timerInfo);
        }
    }

    private void Run(int timerId, TimerInfo timerInfo)
    {
        switch (timerInfo.TimerType)
        {
            case TimerType.OnceTimer:
                {
                    //发送消息
                    EventManager.Instance.TimerNotify(timerId, null);
                    break;
                }
            case TimerType.RepeatedTimer:
                {
                    long timeNow = GetNow(timerInfo.TimeScale);
                    if (timerInfo.EndTime > timeNow)
                    {
                        timerInfo.StartTime = timerInfo.StartTime + timerInfo.Interval;
                        AddTimer(timerId, ref timerInfo);
                    }
                    decimal t = (timerInfo.EndTime - timeNow) / 1000.0m;
                    //发送消息
                    EventManager.Instance.TimerNotify(timerId, (int)Math.Ceiling(t));
                    break;
                }
        }
    }
    #endregion


}

public struct TimerInfo
{
    public TimerInfo(long startTime, long endTime, long interval, TimerType timerType, bool timeScale)
    {
        this.StartTime = startTime;
        this.EndTime = endTime;
        this.Interval = interval;
        this.TimerType = timerType;
        this.TimeScale = timeScale;
    }

    public long Interval;

    public long StartTime;

    public long EndTime;

    public TimerType TimerType;

    public bool TimeScale;
}

public enum TimerType
{
    None,
    OnceTimer,
    RepeatedTimer,
}
