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
            case "SpreadOut":
                _dataSource.state = CARD_STATE.EXTRACT;
                break;
            case "DrawMoveTo":
                _dataSource.state = CARD_STATE.NOT_OPEN;
                break;
            case "ShowCard":

                break;
            case "HideCard":
                _dataSource.state = CARD_STATE.OPEN;
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
            case CARD_STATE.EXTRACT:
                EventManager.Event("DrawCard", _dataSource.index);
                break;
            case CARD_STATE.NOT_OPEN:
                _dataSource.state = CARD_STATE.OPEN;
                cardAni.Turn();
                break;
            case CARD_STATE.OPEN:
                EventManager.Event("ShowCard", this);
                break;
            case CARD_STATE.SHOW:

                break;
            default:
                Debug.Log(_dataSource.index);
                break;
        }
    }

    public void ResetState()
    {
        _dataSource.state = CARD_STATE.NULL;
    }

    public void Draw(Action onComplete = null)
    {
        Vector3 d = transform.TransformPoint(new Vector3(0,0.4f,0));
        cardAni.MoveTo(d, "Draw",new iTweenParameter() { 
            time = 0.5f,
        });
        cardAni.RotateTo(Vector3.zero, "", new iTweenParameter()
        {
            time = 0.2f,
            delay = 0.2f,
            islocal = true,
        });
        cardAni.Once("onComplete", (tag) =>
        {
            if ((string)tag == "Draw")
            {
                onComplete?.Invoke();
            }
        });
    }

    public void DrawMoveTo(Vector3 position)
    {
        cardAni.MoveTo(position, "DrawMoveTo", new iTweenParameter()
        {
            speed = 1,
        });
    }

    //摊开
    public void SpreadOut(int index, Vector3 meshSize)
    {

        int count = Card.AllCard.Length;

        float interval = 1.4f / count;

        float x = (index - count) * interval;
        float y = 0;

        double r = Math.Atan2(meshSize.z, interval);

        if (index != count)
        {
            y = (float)(Math.Sin(r) * (meshSize.x / 2));
        }

        Vector3 vector = transform.parent.TransformPoint(new Vector3(x, 0, - y));

        cardAni.MoveTo(vector, "SpreadOut", new iTweenParameter() {
            speed = 1,
            //delay = 0.01f * (index - 1),
            //looktarget = Vector3.zero,
            //axis = "y",
            //up = -Vector3.forward,
        });

        if (index != count)
        {
            cardAni.RotateAdd(new Vector3(0, (float)(r * 180 / Math.PI), 0), "", new iTweenParameter()
            {
                //delay = 0.01f * (index - 1)
            });
        }
    }

    public void RotateTo(Vector3 eulerAngles)
    {
        cardAni.RotateTo(eulerAngles);
    }

    public void Show(Vector3 position, Vector3 eulerAngles, Vector3 lossyScale)
    {
        _dataSource.state = CARD_STATE.SHOW;
        cardAni.MoveTo(position, "ShowCard");
        cardAni.RotateTo(eulerAngles);
        cardAni.ScaleTo(lossyScale);
    }

    public void Hide(Vector3 position, Vector3 eulerAngles, Vector3 lossyScale)
    {
        cardAni.MoveTo(position , "HideCard");
        cardAni.RotateTo(eulerAngles);
        cardAni.ScaleTo(lossyScale);
    }
}
