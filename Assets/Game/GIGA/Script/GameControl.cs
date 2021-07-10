using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
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
            textTime.text = value.ToString();
        }
    }

    public Text textTime = null;

    public Text textScoreLeft = null;

    public Text textScoreRight = null;

    public List<GameStage> stageList = new List<GameStage>();

    public List<Piece> pieces = new List<Piece>();

    void Start()
    {
        EventManager.On("Init", Init);

        Init(null);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            pieces[0].isSelected = true;
            pieces[1].isSelected = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            pieces[0].isSelected = false;
            pieces[1].isSelected = true;
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            pieces[2].isSelected = true;
            pieces[3].isSelected = false;
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            pieces[2].isSelected = false;
            pieces[3].isSelected = true;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            for (int i = 0; i < 2; i++)
            {
                if (pieces[i].isSelected)
                {
                    pieces[i].station = stageList[0].stationLeft;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            for (int i = 0; i < 2; i++)
            {
                if (pieces[i].isSelected)
                {
                    pieces[i].station = stageList[1].stationLeft;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 0; i < 2; i++)
            {
                if (pieces[i].isSelected)
                {
                    pieces[i].station = stageList[2].stationLeft;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int i = 0; i < 2; i++)
            {
                if (pieces[i].isSelected)
                {
                    pieces[i].station = stageList[3].stationLeft;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            for (int i = 2; i < 4; i++)
            {
                if (pieces[i].isSelected)
                {
                    pieces[i].station = stageList[0].stationRight;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            for (int i = 2; i < 4; i++)
            {
                if (pieces[i].isSelected)
                {
                    pieces[i].station = stageList[1].stationRight;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            for (int i = 2; i < 4; i++)
            {
                if (pieces[i].isSelected)
                {
                    pieces[i].station = stageList[2].stationRight;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            for (int i = 2; i < 4; i++)
            {
                if (pieces[i].isSelected)
                {
                    pieces[i].station = stageList[3].stationRight;
                }
            }
        }
    }

    public void Init(object obj)
    {
        Timer.CloseAll();

        time = 30;

        pieces.ForEach((piece) =>
        {
            piece.Init();
        });

        stageList.ForEach((stage) =>
        {
            stage.Init();
        });

        Timer.Loop(1, MainLoop, "MainLoop");
    }

    private void MainLoop()
    {
        stageList.ForEach((stage) =>
        {
            stage.Settlement();
        });

        time -= 1;

        UpdateUI();

        if (time <= 0)
        {
            Timer.CloseAll();
            Debug.Log("游戏结束，比分 " + textScoreLeft.text + ":" + textScoreRight.text);

            GameOverUI.Show(float.Parse(textScoreLeft.text), float.Parse(textScoreRight.text));
        }
    }

    public void UpdateUI()
    {
        float scoreLeft = 0;
        float scoreRight = 0;
        stageList.ForEach((stage) =>
        {
            scoreLeft += stage.stationLeft.score;
            scoreRight += stage.stationRight.score;
        });
        textScoreLeft.text = scoreLeft.ToString();
        textScoreRight.text = scoreRight.ToString();
    }
}
