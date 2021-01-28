using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceVO
{
    #region field
    private int x = 0;
    private int y = 0;
    #endregion

    #region properity
    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }

    public Vector2 Position
    {
        get
        {
            return new Vector2(x, y);
        }

        set
        {
            x = (int)value.x;
            y = (int)value.y;
        }
    }
    #endregion
}
