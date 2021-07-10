using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameObject selected = null;

    /// <summary>
    /// 是否选中
    /// </summary>
    private bool _isSelected = false;
    public bool isSelected
    {
        get
        {
            return _isSelected;
        }
        set
        {
            _isSelected = value;

            selected.SetActive(value);
        }
    }

    private Station _station = null;
    public Station station
    {
        set
        {
            if (_station != value)
            {
                if (_station != null)
                {
                    Leave(_station);
                }
                _station = value;
                Enter(_station);
            }
        }
    }

    private Vector3 initialPos;

    void Awake()
    {
        initialPos = transform.position;
    }

    public void Init()
    {
        transform.position = initialPos;

        isSelected = false;

        _station = null;
    }

    public void Enter(Station station)
    {
        station.AddPiece(this);
    }

    public void Leave(Station station)
    {
        station.RemovePiece(this);
    }
}
