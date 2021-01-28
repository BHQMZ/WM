using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVO
{
    int _money = 30;
    int _buyCount = 30;
    string _shaderName = "";
    private List<Vector2> _checksRecord = new List<Vector2>();

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
    /// 设置格子记录
    /// </summary>
    /// <param name="vs"></param>
    public void SetChecks(int[] vs)
    {
        _checksRecord.Clear();
        if (vs.Length >= 2)
        {
            for (int i = 0; i < vs.Length; i+=2)
            {
                _checksRecord.Add(new Vector2(vs[i], vs[i + 1]));
            }
        }
    }

    /// <summary>
    /// 获取格子的获取记录
    /// </summary>
    /// <returns></returns>
    public List<Vector2> GetChecks()
    {
        return this._checksRecord;
    }

    /// <summary>
    /// 购买格子
    /// </summary>
    /// <param name="check1"></param>
    /// <param name="check2"></param>
    public void BuyChecks(Vector2 check1, Vector2 check2)
    {
        _checksRecord.Add(check1);
        _checksRecord.Add(check2);
    }
}
