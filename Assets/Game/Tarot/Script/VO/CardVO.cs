using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌状态
/// </summary>
public enum CARD_STATE
{
    /// <summary>
    /// 未设置
    /// </summary>
    NULL = 0,
    /// <summary>
    /// 可抽取的状态（手牌中）
    /// </summary>
    EXTRACT = 1,
    /// <summary>
    /// 未翻开的状态（抽取后未翻转）
    /// </summary>
    NOT_OPEN = 2,
    /// <summary>
    /// 翻开状态
    /// </summary>
    OPEN = 3,
    /// <summary>
    /// 显示状态（展示状态）
    /// </summary>
    SHOW = 4,
}

public class CardVO : EventDispatcher
{
    private int _index;

    private string _name;

    private string _cardFront;

    private string _cardBack;

    private bool _isReversed;

    private CARD_STATE _state = CARD_STATE.NULL;

    public int index {
        get 
        {
            return _index;
        }
        set
        {
            if(_index != value)
            {
                _index = value;

                Event("index", value);
            }
        }
    }

    public string name {
        get 
        {
            return _name;
        }
        set
        {
            if (_name != value)
            {
                _name = value;

                Event("name", value);
            }
        }
    }

    public string cardFront
    {
        get
        {
            return _cardFront;
        }
        set
        {
            if (_cardFront != value)
            {
                _cardFront = value;

                Event("cardFront", value);
            }
        }
    }

    public string cardBack
    {
        get
        {
            return _cardBack;
        }
        set
        {
            if (_cardBack != value)
            {
                _cardBack = value;

                Event("cardBack", value);
            }
        }
    }

    /// <summary>
    /// 是否为为逆位
    /// </summary>
    public bool isReversed
    {
        get
        {
            return _isReversed;
        }
        set
        {
            if (_isReversed != value)
            {
                _isReversed = value;

                Event("isReversed", value);
            }
        }
    }

    public CARD_STATE state
    {
        get
        {
            return _state;
        }
        set
        {
            if (_state != value)
            {
                _state = value;

                Event("state", value);
            }
        }
    }
}
