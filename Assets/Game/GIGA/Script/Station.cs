using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    public List<Transform> pieceTransforms = new List<Transform>();

    /// <summary>
    /// 收益
    /// </summary>
    public float profit = 1;
    /// <summary>
    /// 收益速率（默认每秒收益一次）
    /// </summary>
    public float rate = 1;

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

    private float _progress = 0;
    /// <summary>
    /// 进度
    /// </summary>
    public float progress
    {
        set
        {
            if (_progress != value)
            {
                _progress = value;
                UpdateProgress(value);
            }
        }
    }

    private List<Piece> pieces = new List<Piece>();

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //void OnMouseDown()
    //{
    //    gainSpeed += 1;

    //    Debug.Log("加油：" + gainSpeed);
    //}

    private void UpdateProgress(float progress)
    {
        if (progress == 3)
        {
            Timer.Loop(rate, GainScore, this, "GainScore");
        }
        else
        {
            Timer.Close(this, "GainScore");
        }
    }

    private void GainScore()
    {
        _score += profit;
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
}
