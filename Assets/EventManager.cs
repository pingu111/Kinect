using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All the types of event that can be raised
/// </summary>
public enum MyEventTypes
{
    STATE_CHANGED,
    ORIENTATION_STATE_CHANGED
}

public delegate void Callback();

public delegate void Callback<T>(T arg);



public static class EventManager
{

    /// <summary>
    /// Dictionnary that translate the events to callbacks, methods to call 
    /// when the event is raised
    /// </summary> 
    private static Dictionary<MyEventTypes, List<Delegate>> dicoEventAction = new Dictionary<MyEventTypes, List<Delegate>>();

    /// <summary>
    /// Subscribe a method to an event
    /// </summary>
    /// <param name="even">The event</param>
    /// <param name="method">The method</param>
    public static void addActionToEvent(MyEventTypes even, Callback method)
    {
        if (!dicoEventAction.ContainsKey(even))
        {
            dicoEventAction.Add(even, new List<Delegate>());
        }
        dicoEventAction[even].Add(method);
    }

    /// <summary>
    /// Subscribe a method to an event
    /// </summary>
    /// <param name="even">The event</param>
    /// <param name="method">The method</param>
    public static void addActionToEvent<T>(MyEventTypes even, Callback<T> method)
    {
        if (!dicoEventAction.ContainsKey(even))
        {
            dicoEventAction.Add(even, new List<Delegate>());
        }
        dicoEventAction[even].Add(method);
    }

    /// <summary>c
    /// Unsubscribe a method to an event
    /// </summary>
    /// <param name="even">The event to unsubscribe</param>
    /// <param name="method">The method to unsubscribe</param>
    public static void removeActionFromEvent(MyEventTypes even, Callback method)
    {
        if (dicoEventAction.ContainsKey(even))
        {
            dicoEventAction[even].Remove(method);
        }
    }

    /// <summary>c
    /// Unsubscribe a method to an event
    /// </summary>
    /// <param name="even">The event to unsubscribe</param>
    /// <param name="method">The method to unsubscribe</param>
    public static void removeActionFromEvent<T>(MyEventTypes even, Callback<T> method)
    {
        if (dicoEventAction.ContainsKey(even))
        {
            dicoEventAction[even].Remove(method);
        }
    }

    /// <summary>
    /// Raise an event, so it will trigger all the subscribed methods
    /// </summary>
    /// <param name="eventToCall">The event to raise</param>
    public static void raise(MyEventTypes eventToCall)
    {
       // Debug.Log("Raise " + eventToCall);
        foreach (Delegate d in dicoEventAction[eventToCall])
        {
            Callback c = (Callback)d;
            if (c != null)
                c();
        }
    }

    /// <summary>
    /// Raise an event, so it will trigger all the subscribed methods
    /// </summary>
    /// <param name="eventToCall">The event to raise</param>
    /// <param name="arg">The argument that we pass with the event</param>

    public static void raise<T>(MyEventTypes eventToCall, T arg)
    {
        //Debug.Log("Raise " + eventToCall+" "+arg);

        foreach (Delegate d in dicoEventAction[eventToCall])
        {
            Callback<T> c = (Callback<T>)d;
            if (c != null)
                c(arg);
        }
    }
}
