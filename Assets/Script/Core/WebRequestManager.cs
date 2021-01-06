using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class WebRequestManager
{
    class RequestInfo { public UnityWebRequest request;  public UnityAction<UnityWebRequest> action; public UnityAction<float> progressAction; }

    static List<RequestInfo> RequestList = new List<RequestInfo>();

    /// <summary>
    /// 是否进行网络请求
    /// </summary>
    public static bool IsRequest
    {
        get
        {
            return RequestList.Count > 0;
        }
    }

    /// <summary>
    /// 网络请求开始携程
    /// </summary>
    /// <returns></returns>
    public static IEnumerator WebRequestStart()
    {
        Debug.Log("开始网络请求");

        RequestInfo info = RequestList[0];
        RequestList.RemoveAt(0);

        info.request.SendWebRequest();

        while (!info.request.isDone)
        {
            info.progressAction?.Invoke(info.request.downloadProgress);
            yield return null;
        }

        info.progressAction?.Invoke(info.request.downloadProgress);

        if (info.request.isNetworkError || info.request.isHttpError)
        {
            Debug.LogError("网络请求错误：" + info.request.error);
        }
        else
        {
            Debug.Log("网络请求完成");

            info.action?.Invoke(info.request);
        }
    }

    public static void Request(UnityWebRequest request, UnityAction<UnityWebRequest> action = null, UnityAction<float> progressAction = null)
    {
        RequestList.Add(new RequestInfo() { request = request, action = action, progressAction = progressAction });
    }
}
