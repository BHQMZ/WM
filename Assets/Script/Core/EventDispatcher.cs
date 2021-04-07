
using System;
using System.Collections.Generic;

public class EventDispatcher
{
    private Dictionary<string, Action<object>> onEvents = new Dictionary<string, Action<object>>();
    private Dictionary<string, Action<object>> onceEvents = new Dictionary<string, Action<object>>();

    public void Event(string type, object data)
    {
        if (onEvents.ContainsKey(type))
        {
            onEvents[type]?.Invoke(data);
        }
        if (onceEvents.ContainsKey(type))
        {
            onceEvents[type]?.Invoke(data);
            onceEvents[type] = null;
        }
    }

    public void On(string type, Action<object> action)
    {
        if(onEvents.ContainsKey(type))
        {
            onEvents[type] += action;
        }
        else
        {
            onEvents[type] = action;
        }
    }

    public void Once(string type, Action<object> action)
    {
        if (onceEvents.ContainsKey(type))
        {
            onceEvents[type] += action;
        }
        else
        {
            onceEvents[type] = action;
        }
    }

    public void Off(string type, Action<object> action)
    {
        if (onEvents.ContainsKey(type))
        {
            onEvents[type] -= action;
        }
        if (onceEvents.ContainsKey(type))
        {
            onceEvents[type] -= action;
        }
    }

    public void OffAll()
    {
        onEvents.Clear();
        onceEvents.Clear();
    }
}
