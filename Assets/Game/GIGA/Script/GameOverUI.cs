using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : DialogBase<GameOverUI>
{
    public Text title;

    public Text score;

    public static string url = "Dialog/GameOverUI";

    void Awake()
    {

    }

    void Start()
    {

    }
    void Update()
    {

    }

    public void onClickHide()
    {
        Hide();

        EventManager.Event("Init",null);
    }

    public static void Show(float score1, float score2)
    {
        Show((alert) => {
            if (score1 > score2)
            {
                alert.title.text = "红方获胜";
            }
            else if(score1 < score2)
            {
                alert.title.text = "蓝方获胜";
            }
            else
            {
                alert.title.text = "平局";
            }

            alert.score.text = score1 + ":" + score2;
        });
    }
}
