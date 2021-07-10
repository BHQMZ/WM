using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartUI : DialogBase<GameStartUI>
{
    public static string url = "Dialog/GameStartUI";

    public void onClickHide()
    {
        //Hide();

        //EventManager.Event("Init", null);
        LoadSceneManager.Switch("GameScene", true);
    }

    public static void Show()
    {
        Show((alert) => {

        });
    }
}