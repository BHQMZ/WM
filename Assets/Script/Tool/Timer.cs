using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private class TimeInfo { public float time; public Action completeAction; public bool isLoop = false; }
    private static List<TimeInfo> TimeList = new List<TimeInfo>();

    /// <summary>
    /// 是否开启新的携程进行计时
    /// </summary>
    public static bool IsTime
    {
        get
        {
            return TimeList.Count > 0;
        }
    }

    /// <summary>
    /// 加载开始携程
    /// </summary>
    /// <returns></returns>
    public static IEnumerator TimeStart()
    {
        TimeInfo info = TimeList[0];
        TimeList.RemoveAt(0);

        if (info.isLoop)
        {
            while (true)
            {
                yield return new WaitForSeconds(info.time);
                info.completeAction?.Invoke();
            }
        }
        else
        {
            yield return new WaitForSeconds(info.time);
            info.completeAction?.Invoke();
        }
    }

    /// <summary>
    /// 设置一次性定时器
    /// </summary>
    public static void One(float time, Action completeAction)
    {
        TimeList.Add(new TimeInfo() { time = time, completeAction = completeAction });
    }

    /// <summary>
    /// 设置循环定时器
    /// </summary>
    public static void Loop(float time, Action completeAction)
    {
        TimeList.Add(new TimeInfo() { time = time, completeAction = completeAction, isLoop = true });
    }
}
