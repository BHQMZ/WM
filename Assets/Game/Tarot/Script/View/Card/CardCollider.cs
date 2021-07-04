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
        cardAni.On("onUpdate", onAniUpdate);
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

    private void onAniUpdate(object tag)
    {
        switch ((string)tag)
        {
            case "Float":
                transform.LookAt(transform.parent);
                Vector3 vector = transform.localEulerAngles;
                vector.y = 90;
                transform.localEulerAngles = vector;
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
    public void SpreadOut(Vector3 position, Vector3 rotation)
    {

        cardAni.MoveTo(position, "SpreadOut", new iTweenParameter() {
            speed = 1,
            //delay = 0.01f * (index - 1),
            //looktarget = Vector3.zero,
            //axis = "y",
            //up = -Vector3.forward,
        });

        cardAni.RotateAdd(rotation, "", new iTweenParameter()
        {
            //delay = 0.01f * (index - 1)
        });
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

    public void Float(Vector3 lookTarget,Vector3[] path)
    {
        transform.position = path[0];
        cardAni.MoveTo(path, "Float", new iTweenParameter()
        {
            looptype = iTween.LoopType.loop,
            easetype = iTween.EaseType.linear,

            time = 2,

            //islocal = true,
        });
    }
}
