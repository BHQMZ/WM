using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private class TimeInfo { public float time; public Action completeAction; public bool isLoop = false; public string tag; public bool isClose = false; }
    private static List<TimeInfo> TimeList = new List<TimeInfo>();

    //开始计时的计时器列表
    private static List<TimeInfo> TimeStartList = new List<TimeInfo>();

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

        if (info.isClose)
        {
            yield break;
        }

        TimeStartList.Add(info);

        if (info.isLoop)
        {
            while (true)
            {
                yield return new WaitForSeconds(info.time);
                if (info.isClose)
                {
                    TimeStartList.Remove(info);
                    yield break;
                }
                info.completeAction?.Invoke();
            }
        }
        else
        {
            yield return new WaitForSeconds(info.time);
            TimeStartList.Remove(info);
            if (info.isClose)
            {
                yield break;
            }
            info.completeAction?.Invoke();
        }
    }

    /// <summary>
    /// 设置一次性定时器
    /// </summary>
    public static void One(float time, Action completeAction, string tag = "")
    {
        TimeList.Add(new TimeInfo() { time = time, completeAction = completeAction, tag = tag });
    }

    /// <summary>
    /// 设置循环定时器
    /// </summary>
    public static void Loop(float time, Action completeAction, string tag = "")
    {
        TimeList.Add(new TimeInfo() { time = time, completeAction = completeAction, isLoop = true, tag = tag });
    }

    /// <summary>
    /// 关闭指定定时器
    /// </summary>
    public static void Close(string tag = "")
    {
        TimeList.ForEach((info) => {
            if (info.tag == tag)
            {
                info.isClose = true;
            }
        });
        TimeStartList.ForEach((info) => {
            if (info.tag == tag)
            {
                info.isClose = true;
            }
        });
    }

    /// <summary>
    /// 关闭所有定时器
    /// </summary>
    public static void CloseAll()
    {
        TimeList.ForEach((info) => {
            info.isClose = true;
        });
        TimeStartList.ForEach((info)=> {
            info.isClose = true;
        });
    }
}
