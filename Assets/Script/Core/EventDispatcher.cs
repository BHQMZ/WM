
using System;
using System.Collections.Generic;

public class EventDispatcher
{
    private Dictionary<string, Action<object>> events = new Dictionary<string, Action<object>>();

    public Dictionary<string, Action<object>> EventList
    {
        get
        {
            return events;
        }
    }

    protected void Event(string type, object data)
    {
        events[type]?.Invoke(data);
    }

    public void On(string type, Action<object> action)
    {
        //防止重复添加
        Off(type, action);

        events[type] += action;
    }

    public void Once(string type, Action<object> action)
    {
        Action<object> act = (object obj) =>
        {
            action?.Invoke(obj);
            //Off(type,act);
        };
        On(type, act);
    }

    public void Off(string type, Action<object> action)
    {
        events[type] -= action;
    }
}
