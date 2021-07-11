using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Station : MonoBehaviour
{
    public Text textProfit = null;
    public Text textAdd = null;

    public List<Transform> pieceTransforms = new List<Transform>();

    /// <summary>
    /// 收益
    /// </summary>
    private float profit = 1;

    private float _score = 0;
    public float score
    {
        get
        {
            return _score;
        }
    }

    /// <summary>
    /// 获取速度
    /// </summary>
    private float _gainSpeed = 0;
    public float gainSpeed
    {
        get
        {
            return _gainSpeed;
        }
    }

    /// <summary>
    /// 是否运作
    /// </summary>
    private bool _isRun = false;

    public bool isRun
    {
        set
        {
            if (_isRun != value)
            {
                _isRun = value;

                ChangeRunType();
            }
        }
    }

    private List<Piece> pieces = new List<Piece>();

    void Start()
    {
        textProfit.rectTransform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    void Update()
    {
        textProfit.text = profit.ToString();
    }

    public void Init()
    {
        _isRun = false;

        profit = 1;

        _gainSpeed = 0;

        _score = 0;

        pieces.Clear();
    }

    public void Settlement()
    {
        if (_isRun)
        {
            if (profit > 0)
            {
                GainScore();
            }
        }
    }

    public bool IsAdd()
    {
        return _isRun && profit > 0;
    }

    public void AddProfit()
    {
        profit += 1;
    }

    public void ReduceProfit()
    {
        if (profit > 0)
        {
            profit -= 1;
        }
    }

    private void GainScore()
    {
        _score += profit;
        PlayAddAni("+" + profit);
        Debug.Log("+" + profit);
    }

    public void AddPiece(Piece piece)
    {
        pieces.Add(piece);
        UpdatePiece();
    }

    public void RemovePiece(Piece piece)
    {
        pieces.Remove(piece);
        UpdatePiece();
    }

    private void UpdatePiece()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            Piece piece = pieces[i];
            Vector3 pos = pieceTransforms[i].position;
            piece.transform.position = pos;
        }

        _gainSpeed = pieces.Count;
    }

    private void PlayAddAni(string add)
    {
        textAdd.text = add;

        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);

        textAdd.rectTransform.position = pos;

        pos.y += 60;

        Hashtable args = new Hashtable();

        args.Add("position", pos);
        args.Add("time", 0.3);
        args.Add("oncomplete", "onTweenComplete");
        args.Add("oncompletetarget", gameObject);

        iTween.MoveTo(textAdd.gameObject, args);
    }

    private void onTweenComplete()
    {
        textAdd.text = "";
    }

    private void ChangeRunType()
    {
        if (_isRun)
        {
            Debug.Log("接上");
        }
        else
        {
            Debug.Log("断开");
        }
    }
}
