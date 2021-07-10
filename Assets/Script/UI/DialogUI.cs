using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogUI : MonoBehaviour
{
    void Awake()
    {
        if (DialogManager.PresentDialogUI)
        {
            return;
        }
        //将当前启用的弹窗UI设置为PresentDialogUI
        DialogManager.PresentDialogUI = this;
        //默认隐藏
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        if(DialogManager.PresentDialogUI == this) DialogManager.PresentDialogUI = null;
    }

    /// <summary>
    /// 显示弹窗
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 隐藏弹窗
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 隐藏所有弹窗
    /// </summary>
    public void HideAll()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.SetParent(null, false);
            child.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
