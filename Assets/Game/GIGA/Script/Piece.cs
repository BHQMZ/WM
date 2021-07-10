using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    /// <summary>
    /// 是否选中
    /// </summary>
    public bool _isSelected = false;
    public bool isSelected 
    {
        get
        {
            return _isSelected;
        }
        set
        {
            _isSelected = value;
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

    void Update()
    {
        
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
