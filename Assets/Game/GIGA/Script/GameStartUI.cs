using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartUI : DialogBase<GameStartUI>
{
    public static string url = "Dialog/GameStartUI";

    void Start()
    {
        SoundManager.Instance.PlayBGMSound("SFX_Login");
    }



    void OnDestroy()
    {
        // SoundManager.Instance.StopBMGSound();
    }

    public void onClickHide()
    {
        //Hide();

        SoundManager.Instance.PlayBGMSound("Start_Game");
        SoundManager.Instance.PlayBGMSound("Stop_LoginSFX");
        //EventManager.Event("Init", null);
        LoadSceneManager.Switch("GameScene", true);
    }

    public static void Show()
    {
        Show((alert) => {

        });
    }
}