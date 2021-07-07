using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

//切换场景控制类
public class LoadSceneManager
{
    class LoadInfo { public string name; public UnityAction<AsyncOperation> completeAction; public UnityAction<float> progressAction; }

    static List<LoadInfo> LoadList = new List<LoadInfo>();

    /// <summary>
    /// 是否开启新的携程进行加载
    /// </summary>
    public static bool IsLoad
    {
        get
        {
            return LoadList.Count > 0;
        }
    }

    /// <summary>
    /// 加载开始携程
    /// </summary>
    /// <returns></returns>
    public static IEnumerator LoadStart() {

        LoadInfo info = LoadList[0];
        LoadList.RemoveAt(0);

        Debug.Log("开始加载场景：" + info.name);

        AsyncOperation async = SceneManager.LoadSceneAsync(info.name);

        //阻止当加载完成自动切换
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            info.progressAction?.Invoke(async.progress);

            if (async.progress >= 0.9f)
            {
                info.progressAction?.Invoke(1f);
                info.completeAction?.Invoke(async);

                //Scene scene = SceneManager.GetSceneByName(info.name);

                //SceneManager.SetActiveScene(scene);

                Debug.Log("完成加载场景：" + info.name);

                break;
            }

            yield return null;
        }
    }


    public static void Load(string name, UnityAction<AsyncOperation> completeAction = null, UnityAction<float> progressAction = null)
    {
        if (Application.CanStreamedLevelBeLoaded(name))
        {
            LoadList.Add(new LoadInfo() { name = name, completeAction = completeAction, progressAction = progressAction });
        }
        else
        {
            LoadManager.LoadAssetBundle("scene/"+name,(ab)=> {
                LoadList.Add(new LoadInfo() { name = name, completeAction = completeAction, progressAction = (progress) => {
                    progressAction?.Invoke(0.5f + progress/2);
                } });
            },(progress) => {
                progressAction?.Invoke(progress / 2);
            });
        }
    }

    public static void Switch(string name, bool showLoadingScene = false)
    {
        if (showLoadingScene)
        {
            Load("LoadingScene", (operation) => {
                LoadingUI.NextScenes = name;
                operation.allowSceneActivation = true;
            }, (progress) => {
                Debug.Log(progress);
            });
        }
        else
        {
            Load(name, (operation) => {
                operation.allowSceneActivation = true;
            }, (progress) => {
                Debug.Log(progress);
            });
        }
    }
}
