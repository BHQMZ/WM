﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStage : MonoBehaviour
{
    /// <summary>
    /// 左车站
    /// </summary>
    public Station stationLeft = null;
    /// <summary>
    /// 右车站
    /// </summary>
    public Station stationRight = null;
    /// <summary>
    /// 轨道
    /// </summary>
    public Track track = null;

    private float speedMultiplier = 4;

    private float progress = 0;

    void Start()
    {

    }

    void Update()
    {
        if (track == null)
        {
            Debug.LogError("缺少轨道");
            return;
        }

        if (stationLeft == null)
        {
            Debug.LogError("缺少左车站");
            return;
        }

        if (stationRight == null)
        {
            Debug.LogError("缺少右车站");
            return;
        }

        UpdateProgress();
    }

    public void Init()
    {
        stationLeft.Init();
        stationRight.Init();
        track.Init();

        progress = 0;
    }

    private void UpdateProgress()
    {
        float speed = stationRight.gainSpeed - stationLeft.gainSpeed;
        float target = progress + speed;
        if (target > 3)
        {
            target = 3;
        }
        if (target < -3)
        {
            target = -3;
        }
        progress = Mathf.MoveTowards(progress, target, Mathf.Abs(speed) * speedMultiplier * Time.deltaTime);

        stationRight.isRun = progress == 3;

        stationLeft.isRun = progress == -3;

        track.progress = progress;
    }

    /// <summary>
    /// 结算数据
    /// </summary>
    public void Settlement()
    {
        stationLeft.Settlement();
        stationRight.Settlement();
        // if (stationLeft.IsAdd())
        // {
        //     stationLeft.ReduceProfit();
        //     stationRight.AddProfit();
        // }
        // if (stationRight.IsAdd())
        // {
        //     stationLeft.AddProfit();
        //     stationRight.ReduceProfit();
        // }
    }
}
