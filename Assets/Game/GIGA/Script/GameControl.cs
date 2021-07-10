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

    private int indexLeft = -1;
    private int indexRight = -1;

    void Start()
    {
        EventManager.On("Init", Init);

        Init(null);
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.J))
        // {
        //     pieces[0].isSelected = !pieces[0].isSelected;
        //     pieces[1].isSelected = !pieces[1].isSelected;
        // }

        // if (Input.GetKeyDown(KeyCode.B))
        // {
        //     pieces[0].isSelected = !pieces[0].isSelected;
        //     pieces[1].isSelected = !pieces[1].isSelected;
        // }

        // if (Input.GetKeyDown(KeyCode.KeypadEnter))
        // {
        //     pieces[2].isSelected = !pieces[2].isSelected;
        //     pieces[3].isSelected = !pieces[3].isSelected;
        // }

        // if (Input.GetKeyDown(KeyCode.RightArrow))
        // {
        //     pieces[2].isSelected = !pieces[2].isSelected;
        //     pieces[3].isSelected = !pieces[3].isSelected;
        // }

        if (Input.GetKeyDown(KeyCode.W))
        {
            // for (int i = 0; i < 2; i++)
            // {
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
            // }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            // for (int i = 0; i < 2; i++)
            // {
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
            // }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // for (int i = 2; i < 4; i++)
            // {
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
            // }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // for (int i = 2; i < 4; i++)
            // {
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
            // }
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

        pieces[0].isSelected = true;
        pieces[1].isSelected = false;

        pieces[2].isSelected = true;
        pieces[3].isSelected = false;

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
