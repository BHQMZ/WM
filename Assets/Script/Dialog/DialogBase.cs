using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class DialogBase : MonoBehaviour
{
     
}

public class DialogBase<T> : DialogBase where T : DialogBase
{
    private static T _instance;

    /// <summary>
    /// 显示
    /// </summary>
    public static void Show(UnityAction<T> onComplete = null)
    {
        if (_instance == null)
        {
            Type type = typeof(T);
            string urlStr = type.GetField("url").GetValue(type).ToString();
            LoadManager.LoadGameObject(urlStr, type.Name, (dialog) => {
                if (dialog)
                {
                    _instance = Instantiate(dialog).GetComponent<T>();

                    //DestroyImmediate(dialog, true);

                    DialogManager.Show(_instance);

                    onComplete?.Invoke(_instance);
                }
                else
                {
                    Debug.Log("创建弹窗失败");
                }
            },(progress) => {
                Debug.Log(progress);
            });
        }
        else
        {
            DialogManager.Show(_instance);

            onComplete?.Invoke(_instance);
        }
    }

    /// <summary>
    /// 隐藏
    /// </summary>
    public static void Hide()
    {
        if (_instance != null)
        {
            DialogManager.Hide(_instance);
        }
        else
        {
            Debug.LogError(string.Format("{0}窗口不存在", typeof(T).Name));
        }
    }
}
