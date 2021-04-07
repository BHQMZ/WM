using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class AssetBundleManager {
    private class LoadInfo { public string url; public UnityAction<AssetBundle> completeAction; public UnityAction<float> progressAction; }

    private static Dictionary<string, AssetBundle> ABList = new Dictionary<string, AssetBundle>();
    private static Dictionary<string, AssetBundleCreateRequest> ABCRList = new Dictionary<string, AssetBundleCreateRequest>();
    private static List<LoadInfo> LoadList = new List<LoadInfo>();

    private static AssetBundleManifest manifest;

    private static int LoadCount = 0;

    private static AssetBundleCreateRequest GetRequest(string url)
    {
        if (ABCRList.ContainsKey(url))
        {
            return ABCRList[url];
        }
        else
        {
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(URL + url);
            ABCRList.Add(url, request);
            return request;
        }
    }

    private static string URL
    {
        get
        {
            return Application.streamingAssetsPath + '/';
        }
    }

    private static bool IsWebRequest(string url)
    {
        return url.IndexOf("http://") == 0 || url.IndexOf("https://") == 0 || url.IndexOf("file://") == 0;
    }

    /// <summary>
    /// 是否开启新的携程进行加载
    /// </summary>
    public static bool IsLoad
    {
        get
        {
            return LoadList.Count > 0 && LoadCount <= Environment.ProcessorCount;
        }
    }

    /// <summary>
    /// 获取总AssetBundleManifest
    /// </summary>
    /// <param name="completeAction">获取回调</param>
    private static void LoadAssetBundleManifest(UnityAction<AssetBundleManifest> completeAction = null)
    {
        if(manifest == null)
        {
            Load("StandaloneWindows", (ab) => {
                manifest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

                completeAction?.Invoke(manifest);
            });
        }
        else
        {
            completeAction?.Invoke(manifest);
        }
    }

    /// <summary>
    /// 加载AssetBundle依赖
    /// </summary>
    /// <param name="name">AssetBundle名字</param>
    /// <param name="completeAction">完成回调</param>
    /// <param name="progressAction">进度回调</param>
    public static void LoadDependent(string name, UnityAction completeAction, UnityAction<float> progressAction = null)
    {
        LoadAssetBundleManifest((manifest) => {
            string[] dependencies = manifest.GetDirectDependencies(name);

            if (dependencies.Length > 0)
            {
                Debug.Log("加载" + name + "相关依赖");
                int loadNumber = 0;
                float progressCount = 0;
                for (int i = 0; i < dependencies.Length; i++)
                {
                    float partProgress = 0;
                    Load(dependencies[i], (dependency) =>
                    {
                        loadNumber++;
                        if (loadNumber == dependencies.Length)
                        {
                            completeAction?.Invoke();
                        }
                    },(progress) => {
                        partProgress = progress - partProgress;
                        progressCount += partProgress;
                        progressAction?.Invoke(progressCount / dependencies.Length);
                    });
                }
            }
            else
            {
                completeAction?.Invoke();
            }
        });
    }

    /// <summary>
    /// 通过网络加载AssetBundle
    /// </summary>
    /// <param name="url">AssetBundle地址</param>
    /// <returns></returns>
    private static void LoadByWeb(string url , UnityAction<AssetBundle> action, UnityAction<float> progressAction = null)
    {
        if (ABList.ContainsKey(url))
        {
            progressAction?.Invoke(1f);
            action(ABList[url]);
        }
        else
        {
            UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(url);

            WebRequestManager.Request(uwr, (wr) =>
            {
                AssetBundle asset = DownloadHandlerAssetBundle.GetContent(uwr);
                action(asset);
            }, progressAction);
        }
    }

    /// <summary>
    /// 加载完成后执行
    /// </summary>
    /// <param name="ab"></param>
    /// <param name="info"></param>
    private static void LoadComplete(AssetBundle ab, LoadInfo info)
    {
        if (ab == null)
        {
            Debug.Log("加载AssetBundle失败：" + info.url);
        }
        else
        {
            Debug.Log("加载AssetBundle成功：" + info.url);

            if (!ABList.ContainsKey(info.url)) ABList.Add(info.url, ab);

            info.completeAction?.Invoke(ab);
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

        Debug.Log("开始加载AssetBundle：" + info.url);

        if (IsWebRequest(info.url))
        {
            LoadByWeb(info.url, (ab) => {
                LoadComplete(ab, info);
            }, info.progressAction);
        }
        else
        {
            AssetBundle ab;

            if (ABList.ContainsKey(info.url))
            {
                ab = ABList[info.url];
            }
            else
            {
                AssetBundleCreateRequest request = GetRequest(info.url);

                while (!request.isDone)
                {
                    info.progressAction?.Invoke(request.progress);
                    yield return null;
                }

                ab = request.assetBundle;
            }

            info.progressAction?.Invoke(1f);

            LoadComplete(ab, info);
        }

        LoadCount--;
    }

    /// <summary>
    /// 加载游戏对象
    /// </summary>
    public static void Load(string url, UnityAction<AssetBundle> completeAction = null, UnityAction<float> progressAction = null)
    {
        LoadList.Add(new LoadInfo() { url = url, completeAction = completeAction, progressAction = progressAction });
    }

    /// <summary>
    /// 卸载指定AssetBundle
    /// </summary>
    /// <param name="url">AssetBundle路径</param>
    public static void Unload(string url)
    {
        if (ABList.ContainsKey(url))
        {
            ABList[url].Unload(true);
            ABList.Remove(url);
        }
    }

    /// <summary>
    /// 卸载所有AssetBundle
    /// </summary>
    public static void UnloadAll()
    {
        AssetBundle.UnloadAllAssetBundles(true);
        ABList.Clear();
    }
}
