using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class iTweenParameter
{
    public float speed = 0;
    public float time = 0;
    public float delay = 0;
    public bool isLooktarget = false;
    public Vector3 looktarget;
    public Vector3 up = Vector3.up;
    public string axis;
    public bool islocal = false;
    public iTween.LoopType looptype;
    public iTween.EaseType easetype = iTween.EaseType.easeOutExpo;
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

    private Hashtable createTweenHashtable(string tag, iTweenParameter parameter = null)
    {
        Hashtable args = new Hashtable();

        if (tag != "")
        {
            args.Add("onstart", "onTweenStart");
            args.Add("onstarttarget", gameObject);
            args.Add("onstartparams", tag);

            args.Add("onupdate", "onTweenUpdate");
            args.Add("onupdatetarget", gameObject);
            args.Add("onupdateparams", tag);

            args.Add("oncomplete", "onTweenComplete");
            args.Add("oncompletetarget", gameObject);
            args.Add("oncompleteparams", tag);
        }

        if (parameter != null)
        {
            if (parameter.time != 0) args.Add("time", parameter.time);
            if (parameter.speed != 0) args.Add("speed", parameter.speed);

            args.Add("delay", parameter.delay);
            args.Add("looptype", parameter.looptype);
            args.Add("easetype", parameter.easetype);

            if (parameter.isLooktarget)
            {
                args.Add("looktarget", parameter.looktarget);
                args.Add("up", parameter.up);
                if (parameter.axis != null)
                {
                    args.Add("axis", parameter.axis);
                }
            }

            if (parameter.islocal)
            {
                args.Add("islocal", parameter.islocal);
            }
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

    private void onTweenUpdate(string tag)
    {
        dispatcher.Event("onUpdate", tag);
    }

    public void MoveTo(Vector3[] path, string tag = "", iTweenParameter parameter = null)
    {

        Hashtable args = createTweenHashtable(tag, parameter);

        args.Add("path", path);

        iTween.MoveTo(gameObject, args);
    }

    public void MoveTo(Vector3 target,string tag = "", iTweenParameter parameter = null)
    {

        Hashtable args = createTweenHashtable(tag, parameter);

        args.Add("position", target);

        //args.Add("easetype", iTween.EaseType.linear);

        iTween.MoveTo(gameObject, args);
    }

    public void RotateTo(Vector3 target, string tag = "", iTweenParameter parameter = null)
    {
        Hashtable args = createTweenHashtable(tag, parameter);

        args.Add("rotation", target);

        iTween.RotateTo(gameObject, args);
    }

    public void ScaleTo(Vector3 target, string tag = "", iTweenParameter parameter = null)
    {
        Hashtable args = createTweenHashtable(tag, parameter);

        args.Add("scale", target);

        iTween.ScaleTo(gameObject, args);
    }

    public void RotateAdd(Vector3 amount, string tag = "", iTweenParameter parameter = null)
    {
        Hashtable args = createTweenHashtable(tag, parameter);

        args.Add("amount", amount);

        iTween.RotateAdd(gameObject, args);
    }
}
