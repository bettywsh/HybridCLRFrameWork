using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using System.Threading;

public class TimerManager : MonoSingleton<TimerManager>
{

    public long mServerTimer = 0;
    public float validStartGameTime = 0;
    public long ServerTimer
    {
        get
        {
            return mServerTimer + (long)(Time.realtimeSinceStartup - validStartGameTime) * 1000;
        }
        set
        {
            validStartGameTime = Time.realtimeSinceStartup;
            mServerTimer = value;
        }
    }

    private DateTime dt1970;
    // 线程安全
    public long ClientTimer
    {
        get
        {
            return (DateTime.UtcNow.Ticks - this.dt1970.Ticks) / 10000;
        }
    }

    public override async UniTask Init()
    {
        await base.Init();
        this.dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }


    #region 延时定时器

    public void OnceTimer(int timerId, double time)
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
        TimerInfo timer = new(GetNow(), GetNow() + t, t, TimerType.OnceTimer);
        AddTimer(timerId, ref timer);
    }
    #endregion

    #region 倒计时定时器
    public void RepeatedTimer(int timerId, double time, float interval)
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
        TimerInfo timer = new(GetNow(), GetNow() + t, i, TimerType.RepeatedTimer);
        AddTimer(timerId, ref timer);
    }
    #endregion

    #region 定时器逻辑
    public NativeCollection.MultiMap<long, int> timeId = new(1000);
    public Dictionary<long, TimerInfo> timerInfos = new();
    public Queue<long> timeOutTime = new();
    public Queue<long> timeOutTimes = new();

    public long minTime = long.MaxValue;

    public void Clear(long id)
    {
        if (timerInfos.TryGetValue(id, out TimerInfo timer))
        {
            long allTime = timer.StartTime + timer.Interval;
            if (timeId[allTime] != null)
            {
                timeId.Remove(allTime, (int)id);
            }
            if (timeId.Count > 0)
            {
                //去除最小时间
                foreach (var kv in timeId)
                {
                    long k = kv.Key;
                    if (k > GetNow())
                    {
                        minTime = k;
                        break;
                    }
                }
            }
            else
            {
                minTime = int.MaxValue;
            }
            Remove(id);
        }
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

    private long GetNow()
    {
        return ClientTimer;
    }

    private void AddTimer(int timerId, ref TimerInfo timer)
    {
        long tillTime = timer.StartTime + timer.Interval;
        timeId.Add(tillTime, timerId);
        timerInfos.Add(timerId, timer);
        if (tillTime < minTime)
        {
            minTime = tillTime;
        }
    }

    public void Update()
    {
        if (timeId.Count == 0)
        {
            return;
        }
        long timeNow = GetNow();

        if (timeNow < minTime)
        {
            return;
        }

        //去除最小时间
        foreach (var kv in timeId)
        {
            long k = kv.Key;
            if (k > timeNow)
            {
                minTime = k;
                break;
            }

            timeOutTime.Enqueue(k);
        }

        while (timeOutTime.Count > 0)
        {
            long time = timeOutTime.Dequeue();
            var list = timeId[time];
            for (int i = 0; i < list.Length; ++i)
            {
                long timerId = list[i];
                timeOutTimes.Enqueue(timerId);
            }
            timeId.Remove(time);
        }

        if (timeId.Count == 0)
        {
            minTime = long.MaxValue;
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
                    long timeNow = GetNow();
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

    public async UniTask<bool> WaitAsync(float time, CancellationToken cancellationToken = default(CancellationToken))
    {
        var cannel = await UniTask.Delay(TimeSpan.FromSeconds(time), false, PlayerLoopTiming.Update, cancellationToken).SuppressCancellationThrow();
        return cannel;
    }

}

public struct TimerInfo
{
    public TimerInfo(long startTime, long endTime, long interval, TimerType timerType)
    {
        this.StartTime = startTime;
        this.EndTime = endTime;
        this.Interval = interval;
        this.TimerType = timerType;
    }

    public long Interval;

    public long StartTime;

    public long EndTime;

    public TimerType TimerType;
}

public enum TimerType
{
    None,
    OnceTimer,
    RepeatedTimer,
}