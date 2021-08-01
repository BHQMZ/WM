using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public Text textTime = null;

    public Text textScoreLeft = null;

    public Text textScoreRight = null;

    public Animator StartCountdownAni = null;

    public List<GameStage> stageList = new List<GameStage>();

    public List<Piece> pieces = new List<Piece>();

    /// <summary>
    ///  时间（倒计时）
    /// </summary>
    private float _time = 30;

    public float time
    {
        get
        {
            return _time;
        }
        set
        {
            _time = value;
            float t = Mathf.Ceil(value);
            textTime.text = t.ToString();

            if (t <= 5 &&value == t)
            {
                SoundManager.Instance.PlayBGMSound("SFX_TimeOut");
            }
        }
    }

    /// <summary>
    ///  左边玩家分数
    /// </summary>
    private float _scoreLeft = 0;

    public float scoreLeft
    {
        get
        {
            return _scoreLeft;
        }
        set
        {
            _scoreLeft = value;
            textScoreLeft.text = value.ToString();
        }
    }

    /// <summary>
    ///  右边玩家分数
    /// </summary>
    private float _scoreRight = 0;

    public float scoreRight
    {
        get
        {
            return _scoreRight;
        }
        set
        {
            _scoreRight = value;
            textScoreRight.text = value.ToString();
        }
    }

    private bool isStart = false;

    private int startCountdown = 0;

    void Start()
    {
        EventManager.On("Init", Init);

        Init(null);
    }

    void Update()
    {
        if (!isStart)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Piece piece = pieces[0];
            if (piece.isSelected)
            {
                if (piece.station == null)
                {
                    piece.station = stageList[3].stationLeft;
                }
                else
                {
                    for (int j = 0; j < stageList.Count; j++)
                    {
                        if(piece.station == stageList[j].stationLeft){
                            int index = j - 1;
                            if (index < 0)
                            {
                                index = 3;
                            }
                            piece.station = stageList[index].stationLeft;
                            break;
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Piece piece = pieces[0];
            if (piece.isSelected)
            {
                if (piece.station == null)
                {
                    piece.station = stageList[0].stationLeft;
                }
                else
                {
                    for (int j = 0; j < stageList.Count; j++)
                    {
                        if(piece.station == stageList[j].stationLeft){
                            int index = j + 1;
                            if (index > 3)
                            {
                                index = 0;
                            }
                            piece.station = stageList[index].stationLeft;
                            break;
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Piece piece = pieces[2];
            if (piece.isSelected)
            {
                if (piece.station == null)
                {
                    piece.station = stageList[3].stationRight;
                }
                else
                {
                    for (int j = 0; j < stageList.Count; j++)
                    {
                        if(piece.station == stageList[j].stationRight){
                            int index = j - 1;
                            if (index < 0)
                            {
                                index = 3;
                            }
                            piece.station = stageList[index].stationRight;
                            break;
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Piece piece = pieces[2];
            if (piece.isSelected)
            {
                if (piece.station == null)
                {
                    piece.station = stageList[0].stationRight;
                }
                else
                {
                    for (int j = 0; j < stageList.Count; j++)
                    {
                        if(piece.station == stageList[j].stationRight){
                            int index = j + 1;
                            if (index > 3)
                            {
                                index = 0;
                            }
                            piece.station = stageList[index].stationRight;

                            break;
                        }
                    }
                }
            }
        }

    }

    public void Init(object obj)
    {
        Timer.CloseAll();

        time = 30;

        scoreLeft = 0;
        scoreRight = 0;

        isStart = false;
        startCountdown = 0;

        pieces.ForEach((piece) =>
        {
            piece.Init();
        });

        stageList.ForEach((stage) =>
        {
            stage.Init();
        });

        pieces[0].isSelected = true;
        // pieces[1].isSelected = false;
        pieces[0].station = stageList[3].stationLeft;

        pieces[2].isSelected = true;
        // pieces[3].isSelected = false;
        pieces[2].station = stageList[3].stationRight;

        StartCountdownAni.gameObject.SetActive(true);
        StartCountdownAni.Play("StartCountdown", 0, 0);
        Timer.Loop(1, StartCountdown, "StartCountdown");
        Timer.Loop(0.5f, MainLoop, "MainLoop");

        SoundManager.Instance.PlayBGMSound("Game_BGM");
        SoundManager.Instance.PlayBGMSound("AMB_Sea");

        SoundManager.Instance.PlayBGMSound("AMB_LFP");
        SoundManager.Instance.PlayBGMSound("BGM_LFP");
        //Timer.One(0.0f,()=> {
        //    SoundManager.Instance.PlayBGMSound("AMB_LFP");
        //    SoundManager.Instance.PlayBGMSound("BGM_LFP");
        //});
    }

    private void MainLoop()
    {
        if (!isStart)
        {
            return;
        }

        stageList.ForEach((stage) =>
        {
            stage.Settlement();
        });

        time -= 0.5f;

        UpdateScore();

        if (time <= 0)
        {
            Timer.CloseAll();
            Debug.Log("游戏结束，比分 " + scoreLeft + ":" + textScoreRight.text);

            GameOverUI.Show(scoreLeft, float.Parse(textScoreRight.text));
        }
    }

    public void UpdateScore()
    {
        float score1 = 0;
        float score2 = 0;
        stageList.ForEach((stage) =>
        {
            score1 += stage.stationLeft.score;
            score2 += stage.stationRight.score;
        });
        scoreLeft = score1;
        scoreRight = score2;
    }

    private void StartCountdown()
    {
        startCountdown++;
        if (startCountdown == 3)
        {
            SoundManager.Instance.PlayBGMSound("BGM_LFP_Reset");
            SoundManager.Instance.PlayBGMSound("AMB_LFP_Reset");
        }
        if (startCountdown >= 3)
        {
            isStart = true;

            Timer.Close(this, "StartCountdown");
        }
    }
}
