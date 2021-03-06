﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XLua;

//用于执行各种需要在主线程中才能执行的回调
public class GameProcess : MonoBehaviour
{
    private class CoroutineInfo { public Func<IEnumerator> enumerator; public bool isLoop = false; public Func<bool> func = null; };
    private static List<CoroutineInfo> CoroutineList = new List<CoroutineInfo>();
    private List<CoroutineInfo> RemoveList = new List<CoroutineInfo>();

    public static LuaEnv Luaenv;

    public static GameProcess Instance;

    void Awake()
    {
        if (Instance)
        {
            return;
        }
        Instance = this;

        SetStartCoroutine(AssetBundleManager.LoadStart, true, ()=> { return AssetBundleManager.IsLoad; });
        SetStartCoroutine(GameObjectManager.LoadStart, true, () => { return GameObjectManager.IsLoad; });
        SetStartCoroutine(WebRequestManager.WebRequestStart, true, () => { return WebRequestManager.IsRequest; });
        SetStartCoroutine(LoadSceneManager.LoadStart, true, () => { return LoadSceneManager.IsLoad; });

        SetStartCoroutine(Timer.TimeStart, true, () => { return Timer.IsTime; });

        //Timer.Loop(.1f, () =>
        //{
        //    Debug.Log("Loop");
        //}, "loop");
        //Timer.One(.5f, () =>
        //{
        //    Debug.Log("One");
        //}, "one");

        //Timer.Close("loop");

        //Log.Info(Environment.CurrentDirectory + @"\WM_WwiseProject\GeneratedSoundBanks\Windows\");
        SoundManager.Instance.Init();
        //SoundManager.Instance.PlayBGMSound(1151520883);
    }

    void Start()
    {
        GameLua.Init();
        GameLua.EnterLuaGame();
    }

    void OnDisable()
    {
        CoroutineList.Clear();
    }

    void Update()
    {
        CoroutineList.ForEach(info =>
        {
            if (info.func == null || info.func.Invoke())
            {
                if (!info.isLoop)
                {
                    RemoveList.Add(info);
                }
                StartCoroutine(info.enumerator());
            }
        });

        RemoveList.ForEach(info =>
        {
            CoroutineList.Remove(info);
        });

        RemoveList.Clear();

        SocketManager.SocketEngineInvoke();
    }


    /// <summary>
    /// 设置启动携程
    /// </summary>
    /// <param name="enumerator"></param>
    /// <param name="isLoop">是否循环</param>
    /// <param name="func">根据条件判断是否启动</param>
    public static void SetStartCoroutine(Func<IEnumerator> enumerator, bool isLoop = false, Func<bool> func = null)
    {
        CoroutineList.Add(new CoroutineInfo() { enumerator = enumerator, isLoop = isLoop, func = func });
    }
}
