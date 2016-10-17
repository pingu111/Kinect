using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Geste
{
    protected GesteTypes mType;
    private int currentPositionInPath = -1;
    protected List<CurrentState> statePath = new List<CurrentState>();

    void OnStateChange(CurrentState newState)
    {
        if (newState == statePath[currentPositionInPath + 1])
        {
            currentPositionInPath++;
            if (currentPositionInPath == statePath.Count)
            {
                currentPositionInPath = 0;
                GesteDetected();
            }
        }
        else
        {
            currentPositionInPath = 0;
        }
    }

    private void GesteDetected()
    {
        EventManager.raise<GesteTypes>(MyEventTypes.GESTE_DETECTED, mType);
    }

    protected abstract void OnStart();

    void Start()
    {
        EventManager.addActionToEvent<CurrentState>(MyEventTypes.STATE_CHANGED, OnStateChange);
        OnStart();
    }
}

public enum GesteTypes
{
    SPEAK_TO_THE_HAND,
    HANDS_UP,
    SWIPE_LEFT_WITH_RIGHT_HAND,
    SWIPE_RIGHT_WITH_RIGHT_HAND,
    RUN
}
