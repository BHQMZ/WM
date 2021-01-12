using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    int _money = 30;
    int _buyCount = 30;
    string _shaderName = "";
    List<Vector4> checks = new List<Vector4>();

    /// <summary>
    /// 金币数
    /// </summary>
    public int Money
    {
        get
        {
            return _money;
        }
        set
        {
            _money = value;
        }
    }

    /// <summary>
    /// 可购买次数
    /// </summary>
    public int BuyCount
    {
        get
        {
            return _buyCount;
        }
        set
        {
            _buyCount = value;
        }
    }

    /// <summary>
    /// Shader内的名称
    /// </summary>
    public string ShaderName
    {
        get
        {
            return _shaderName;
        }
        set
        {
            _shaderName = value;
        }
    }

    /// <summary>
    /// 设置格子
    /// </summary>
    /// <param name="vs"></param>
    public void SetChecks(int[] vs)
    {
        checks.Clear();
        if (vs.Length >= 2)
        {
            for (int i = 0; i < vs.Length; i+=2)
            {
                checks.Add(new Vector4(vs[i], vs[i + 1]));
            }
        }
    }

    /// <summary>
    /// 购买格子
    /// </summary>
    /// <param name="check1"></param>
    /// <param name="check2"></param>
    public void BuyChecks(Vector4 check1, Vector4 check2)
    {
        checks.Add(check1);
        checks.Add(check2);
    }
}
