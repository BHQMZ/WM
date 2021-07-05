using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class LoadManager
{
    /// <summary>
    /// 加载AssetBundle
    /// </summary>
    /// <param name="url">AssetBundle路径</param>
    /// <param name="completeAction">加载完成回调</param>
    /// <param name="progressAction">加载进度回调</param>
    public static void LoadAssetBundle(string url, UnityAction<AssetBundle> completeAction = null, UnityAction<float> progressAction = null)
    {
        AssetBundleManager.Load(url, completeAction, progressAction);
    }

    /// <summary>
    /// 加载游戏对象
    /// </summary>
    /// <param name="abUrl">AssetBundle路径</param>
    /// <param name="url">游戏对象名称或路径</param>
    /// <param name="action">加载结束回调</param>
    /// <param name="progressAction">加载进度回调</param>
    public static void LoadGameObject(string abUrl, string url, UnityAction<GameObject> completeAction = null, UnityAction<float> progressAction = null)
    {
        AssetBundleManager.Load(abUrl,(ab) => {
            GameObjectManager.Load(url, ab, completeAction, (progress) => {
                progressAction?.Invoke(progress / 2 + 0.5f);
            });
        },(progress) => {
            progressAction?.Invoke(progress / 2);
        });
    }

    /// <summary>
    /// 加载游戏对象
    /// </summary>
    /// <param name="abUrl">AssetBundle路径</param>
    /// <param name="url">游戏对象名称或路径</param>
    /// <param name="action">加载结束回调</param>
    /// <param name="progressAction">加载进度回调</param>
    public static void LoadGameObject(AssetBundle ab, string url, UnityAction<GameObject> completeAction = null, UnityAction<float> progressAction = null)
    {
        GameObjectManager.Load(url, ab, completeAction, progressAction);
    }

    /// <summary>
    /// 加载图片纹理
    /// </summary>
    /// <param name="url">游戏对象名称或路径</param>
    /// <param name="action">加载结束回调</param>
    /// <param name="progressAction">加载进度回调</param>
    public static void LoadTexture(string url, UnityAction<Texture2D> completeAction = null, UnityAction<float> progressAction = null)
    {
        UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture("file://" + Application.dataPath + "/" + url);
        WebRequestManager.Request(webRequest, (request) => {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            completeAction?.Invoke(texture);
        }, progressAction);
    }
}
