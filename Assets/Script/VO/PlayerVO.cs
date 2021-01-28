using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVO
{
    #region field
    private int money = 30;

    private int buyCount = 30;

    private string shaderName = "";

    private List<Vector2> checksRecord = new List<Vector2>();
    #endregion

    #region properity

    /// <summary>
    /// 金币数
    /// </summary>
    public int Money
    {
        get
        {
            return money;
        }
        set
        {
            money = value;
        }
    }

    /// <summary>
    /// 可购买次数
    /// </summary>
    public int BuyCount
    {
        get
        {
            return buyCount;
        }
        set
        {
            buyCount = value;
        }
    }

    /// <summary>
    /// Shader内的名称
    /// </summary>
    public string ShaderName
    {
        get
        {
            return shaderName;
        }
        set
        {
            shaderName = value;
        }
    }

    /// <summary>
    /// 格子的获取记录
    /// </summary>
    /// <returns></returns>
    public List<Vector2> ChecksRecord
    {
        get
        {
            return checksRecord;
        }
    }

    /// <summary>
    /// 设置格子记录
    /// </summary>
    /// <param name="vs"></param>
    public void SetChecks(int[] vs)
    {
        checksRecord.Clear();
        if (vs.Length >= 2)
        {
            for (int i = 0; i < vs.Length; i += 2)
            {
                checksRecord.Add(new Vector2(vs[i], vs[i + 1]));
            }
        }
    }

    /// <summary>
    /// 购买格子
    /// </summary>
    /// <param name="check1"></param>
    /// <param name="check2"></param>
    public void BuyChecks(Vector2 check1, Vector2 check2)
    {
        checksRecord.Add(check1);
        checksRecord.Add(check2);
    }

    #endregion
}
