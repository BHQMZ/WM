using UnityEngine;

public class DialogManager
{
    /// <summary>
    /// 当前场景的Dialog界面
    /// </summary>
    public static DialogUI PresentDialogUI;

    /// <summary>
    /// 显示弹窗
    /// </summary>
    /// <param name="url">窗口资源地址</param>
    public static void Show(DialogBase dialog)
    {
        if(dialog != null)
        {
            if (PresentDialogUI != null)
            {
                dialog.transform.SetParent(PresentDialogUI.transform, false);

                dialog.gameObject.SetActive(true);

                PresentDialogUI.Show();
            }
            else
            {
                Debug.LogError("该场景不存在Dialog界面，无法打开弹窗");
            }
        }
        else
        {
            Debug.LogError("窗口不存在");
        }
    }

    /// <summary>
    /// 隐藏弹窗
    /// </summary>
    /// <param name="dialog">要隐藏的弹窗</param>
    public static void Hide(DialogBase dialog)
    {
        if (dialog != null)
        {
            if (PresentDialogUI != null)
            {
                dialog.transform.SetParent(null, false);

                dialog.gameObject.SetActive(false);

                PresentDialogUI.Hide();
            }
            else
            {
                Debug.LogError("该场景不存在Dialog界面，无法关闭弹窗");
            }
        }
        else
        {
            Debug.LogError("窗口不存在");
        }
    }

    /// <summary>
    /// 隐藏所有弹窗
    /// </summary>
    public static void HideAll()
    {
        if (PresentDialogUI != null)
        {
            PresentDialogUI.HideAll();
        }
        else
        {
            Debug.LogError("该场景不存在Dialog界面，无法关闭弹窗");
        }
    }
}
