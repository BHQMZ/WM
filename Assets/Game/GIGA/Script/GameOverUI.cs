using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : DialogBase<GameOverUI>
{
    public List<GameObject> titles = new List<GameObject>();

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

            alert.titles.ForEach((title) => {
                title.gameObject.SetActive(false);
            });
            if (score1 > score2)
            {
                alert.titles[0].gameObject.SetActive(true);

                SoundManager.Instance.PlayBGMSound("SFX_GameWin_01");
            }
            else if(score1 < score2)
            {
                alert.titles[1].gameObject.SetActive(true);

                SoundManager.Instance.PlayBGMSound("SFX_GameWin_02");
            }
            else
            {
                alert.titles[2].gameObject.SetActive(true);

                SoundManager.Instance.PlayBGMSound("SFX_GameFail");
            }

            SoundManager.Instance.PlayBGMSound("BGM_LFP");
            SoundManager.Instance.PlayBGMSound("AMB_LFP");

            alert.score.text = "<color=#EF1073>" + score1 + "</color>:<color=#3D1FEE>" + score2 + "</color>";
        });
    }
}
