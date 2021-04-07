using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//enum CARD_TWEEN_TAG
//{
//    DRAW = 1,

//}

public class CardCollider : MonoBehaviour, IPointerClickHandler
{
    public CardAnimator cardAni;

    private CARD_STATE state = CARD_STATE.NULL;

    void Start()
    {
        cardAni.On("onComplete", onAniComplete);
    }

    void Update()
    {
        
    }

    private void onAniComplete(object tag)
    {
        switch ((string)tag)
        {
            case "DrawMoveTo":
                state = CARD_STATE.NOT_OPEN;
                break;
        }
    }

    private CardVO _dataSource;

    public CardVO dataSource
    {
        set
        {
            if (_dataSource != value)
            {
                //数据原改变解除对老数据原的监听
                _dataSource?.Off("state", SetState);

                _dataSource = value;

                //增加状态改变时的监听回调
                _dataSource?.On("state", SetState);
            }
        }
    }

    private void SetState(object state)
    {
        this.state = (CARD_STATE)state;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (state)
        {
            case CARD_STATE.NOT_EXTRACT:
                EventManager.Event("DrawCard", _dataSource.index);
                break;
            case CARD_STATE.NOT_OPEN:
                state = CARD_STATE.OPEN;
                cardAni.Turn();
                break;
            case CARD_STATE.OPEN:

                break;
            default:
                Debug.Log(_dataSource.index);
                break;
        }
    }

    public void Draw(Vector3 position)
    {
        Vector3 d = transform.TransformPoint(new Vector3(0,0.4f,0));
        cardAni.MoveTo(d, "Draw");
        cardAni.Once("onComplete", (tag) =>
        {
            if ((string)tag == "Draw")
            {
                cardAni.MoveTo(position, "DrawMoveTo");
            }
        });
    }

    public void MoveToHand(int index)
    {
        float a = 1;
        float b = 0.5f;

        int count = (Card.AllCard.Length + 2) / 2;

        float x = 0;
        float y = 0;

        if (index >= count)
        {
            x = (count * 2 - index - count / 2) * (a * 2 / (count));
            y = (float)-Math.Pow((1 - x * x / (a * a)) * b * b, 0.5);
        }
        else
        {
            x = (index - count / 2) * (a * 2 / (count));
            y = (float)Math.Pow((1 - x * x / (a * a)) * b * b, 0.5);
        }
        //x²/ a² +y ²/ b²= 1;

        if (index > 19.5f)
        {
            Debug.Log("111");
        }

        Vector3 vector = transform.parent.TransformPoint(new Vector3(x, 0.001f*(index-1), y));

        cardAni.MoveTo(vector,"",new MoveToParameter() { 
            delay = 0.01f * (index - 1),
            //looktarget = Vector3.zero,
            //axis = "y",
            //up = -Vector3.forward,
        });


        //cardAni.RotateTo(Vector3.zero);
    }
}
