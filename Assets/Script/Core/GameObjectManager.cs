using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameObjectManager
{
    class LoadInfo { public string url; public AssetBundle ab; public UnityAction<GameObject> completeAction; public UnityAction<float> progressAction; }

    static List<LoadInfo> LoadList = new List<LoadInfo>();

    static int LoadCount = 0;

    /// <summary>
    /// 是否开启新的携程进行加载
    /// </summary>
    public static bool IsLoad
    {
        get
        {
            return LoadList.Count > 0  && LoadCount <= Environment.ProcessorCount;
        }
    }

    /// <summary>
    /// 加载开始携程
    /// </summary>
    /// <returns></returns>
    public static IEnumerator LoadStart()
    {
        LoadCount++;

        LoadInfo info = LoadList[0];
        LoadList.RemoveAt(0);

        Debug.Log("开始加载GameObject：" + info.url);

        if (info.ab.Contains(info.url))
        {
            AssetBundleRequest abr = info.ab.LoadAssetAsync<GameObject>(info.url);

            while (!abr.isDone)
            {
                info.progressAction.Invoke(abr.progress);
                yield return null;
            }

            info.progressAction.Invoke(abr.progress);

            if (abr.asset == null)
            {
                Debug.Log("加载GameObject失败：" + info.url);
            }
            else
            {
                Debug.Log("加载GameObject成功：" + info.url);
            }

            info.completeAction?.Invoke((GameObject)abr.asset);
        }
        else
        {
            Debug.Log(info.ab.name + "资源包中未包含" + info.url);
        }

        LoadCount--;
    }

    /// <summary>
    /// 加载游戏对象
    /// </summary>
    public static void Load(string url, AssetBundle ab, UnityAction<GameObject> completeAction, UnityAction<float> progressAction)
    {
        float progressCount = 0;
        AssetBundleManager.LoadDependent(ab.name, () => {
            LoadList.Add(new LoadInfo() { 
                url = url, 
                ab = ab, 
                completeAction = completeAction, 
                progressAction = (progress) => {
                    progressAction?.Invoke((progressCount + progress) / (1 + progressCount));
                } 
            });
        },(progress) => {
            progressCount = progress;
            progressAction?.Invoke(progress / 2f);
        });
    }
}
