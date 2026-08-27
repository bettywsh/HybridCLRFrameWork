using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

public class TimeManager : MonoSingleton<TimeManager>
{
    public long mServerTimer = 0;
    public float validStartGameTime = 0;

    public override async UniTask Init()
    {
        await base.Init();
        this.dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        timerDeltaTime = 0;
        timerUnscaledDeltaTime = 0;
    }
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

    /// <summary>
    /// 计时器
    /// </summary>
    private long timerDeltaTime;

    public long TimerDeltaTime
    {
        get
        {
            return timerDeltaTime;
        }
    }
    /// <summary>
    /// 计时器
    /// </summary>
    private long timerUnscaledDeltaTime;

    public long TimerUnscaledDeltaTime
    {
        get
        {
            return timerUnscaledDeltaTime;
        }
    }

    private void Update()
    {
        timerDeltaTime += (int)(Time.deltaTime * 1000);
        timerUnscaledDeltaTime += (int)(Time.unscaledDeltaTime * 1000);
    }
}