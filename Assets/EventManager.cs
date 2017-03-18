using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent { }

// Event manager class taken from https://gist.github.com/stfx/3786466
public class EventManager
{
    static EventManager instance;
    public static EventManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventManager();
            }
            return instance;
        }
    }

    public delegate void EventDelegate<T>(T e) where T : GameEvent;

    readonly Dictionary<Type, Delegate> delegates = new Dictionary<Type, Delegate>();

    public void AddListener<T>(EventDelegate<T> listener) where T : GameEvent
    {
        Delegate d;
        if (delegates.TryGetValue(typeof(T), out d))
        {
            delegates[typeof(T)] = Delegate.Combine(d, listener);
        }
        else
        {
            delegates[typeof(T)] = listener;
        }
    }

    public void RemoveListener<T>(EventDelegate<T> listener) where T : GameEvent
    {
        Delegate d;
        if (delegates.TryGetValue(typeof(T), out d))
        {
            Delegate currentDel = Delegate.Remove(d, listener);

            if (currentDel == null)
            {
                delegates.Remove(typeof(T));
            }
            else
            {
                delegates[typeof(T)] = currentDel;
            }
        }
    }

    public void Raise<T>(T e) where T : GameEvent
    {
        if (e == null)
        {
            throw new ArgumentNullException("e");
        }

        Delegate d;
        if (delegates.TryGetValue(typeof(T), out d))
        {
            EventDelegate<T> callback = d as EventDelegate<T>;
            if (callback != null)
            {
                callback(e);
            }
        }
    }
}