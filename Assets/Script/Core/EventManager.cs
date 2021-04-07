using System;

public class EventManager
{
    private static EventDispatcher _Ins;

    public static EventDispatcher Instance
    {
        get
        {
            if (_Ins == null)
            {
                _Ins = new EventDispatcher();
            }
            return _Ins;
        }
    }

    public static void On(string type, Action<object> action)
    {
        Instance.On(type, action);
    }

    public static void Off(string type, Action<object> action)
    {
        Instance.Off(type, action);
    }

    public static void OffAll()
    {
        Instance.OffAll();
    }

    public static void Event(string type, object data)
    {
        Instance.Event(type, data);
    }
}
