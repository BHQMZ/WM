using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoveToParameter{
    public int speed = 1;
    public float delay = 0;
    public bool isLooktarget = false;
    public Vector3 looktarget;
    public Vector3 up = Vector3.up;
    public string axis;
}

public class CardAnimator : MonoBehaviour
{
    public Animator ani;

    public EventDispatcher dispatcher = new EventDispatcher();

    #region Event
    public void On(string type, Action<object> action)
    {
        dispatcher.On(type, action);
    }

    public void Once(string type, Action<object> action)
    {
        dispatcher.Once(type, action);
    }

    public void Off(string type, Action<object> action)
    {
        dispatcher.Off(type, action);
    }

    public void OffAll()
    {
        dispatcher.OffAll();
    }

    public void Turn()
    {
        ani.SetTrigger("Turn");
    }
    #endregion

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private Hashtable createTweenHashtable(string tag)
    {
        Hashtable args = new Hashtable();

        if (tag != "")
        {
            args.Add("onstart", "onTweenStart");
            args.Add("onstarttarget", gameObject);
            args.Add("onstartparams", tag);

            //args.Add("onupdate", "onTweenComplete");
            //args.Add("onupdatetarget", gameObject);
            //args.Add("onupdateparams", tag);

            args.Add("oncomplete", "onTweenComplete");
            args.Add("oncompletetarget", gameObject);
            args.Add("oncompleteparams", tag);
        }

        return args;
    }

    private void onTweenStart(string tag)
    {
        dispatcher.Event("onStart", tag);
    }

    private void onTweenComplete(string tag)
    {
        dispatcher.Event("onComplete", tag);
    }

    public void MoveTo(Vector3 target,string tag = "", MoveToParameter parameter = null)
    {
        Hashtable args = createTweenHashtable(tag);

        args.Add("position", target);

        if (parameter != null)
        {
            args.Add("speed", parameter.speed);

            args.Add("delay", parameter.delay);

            if (parameter.isLooktarget)
            {
                args.Add("looktarget", parameter.looktarget);
                args.Add("up", parameter.up);
                if (parameter.axis != null)
                {
                    args.Add("axis", parameter.axis);
                }
            }
        }

        //args.Add("easetype", iTween.EaseType.linear);

        iTween.MoveTo(gameObject, args);
    }

    public void RotateTo(Vector3 target)
    {
        Hashtable args = createTweenHashtable(tag);

        args.Add("rotation", target);

        iTween.RotateTo(gameObject, args);
    }
}
