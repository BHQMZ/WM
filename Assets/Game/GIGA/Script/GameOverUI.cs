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

        SoundManager.Instance.PlayBGMSound("SFX_GameAgain");
        SoundManager.Instance.PlayBGMSound("Stop_AMB");
        SoundManager.Instance.PlayBGMSound("Stop_BGM");
        SoundManager.Instance.PlayBGMSound("BGM_LFP_Reset");
        SoundManager.Instance.PlayBGMSound("AMB_LFP_Reset");

        EventManager.Event("Init",null);
    }

    public static void Show(float score1, float score2)
    {
        Show((alert) => {
            if (score1 > score2)
            {
                alert.title.text = "红方获胜";

                SoundManager.Instance.PlayBGMSound("SFX_GameWin");
            }
            else if(score1 < score2)
            {
                alert.title.text = "蓝方获胜";

                SoundManager.Instance.PlayBGMSound("SFX_GameWin");
            }
            else
            {
                alert.title.text = "平局";

                SoundManager.Instance.PlayBGMSound("SFX_GameFail");
            }

            SoundManager.Instance.PlayBGMSound("BGM_LFP");
            SoundManager.Instance.PlayBGMSound("AMB_LFP");

            alert.score.text = score1 + ":" + score2;
        });
    }
}
