using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// pour la creation de tout nouveaux gestes :
/// creer la méthode OnStart et y informer le type dans mType
/// puis ajouter les differents etats avec AddStateToPath
/// </summary>
public abstract class Geste
{
    protected GesteTypes mType;
    private int currentPositionInPath = -1;
    private List<Pair<CurrentState,bool>> statePath = new List<Pair<CurrentState,bool>>();

    protected void AddStateToPath(CurrentState state)
    {
        AddStateToPath(state, true);
    }

    protected void AddStateToPath(CurrentState state, bool isImportantState)
    {
        statePath.Add(new Pair<CurrentState,bool>(state,isImportantState));
    }

    void OnStateChange(CurrentState newState)
    {
        if (newState == statePath[currentPositionInPath + 1].First)
        {
            currentPositionInPath++;
            if (currentPositionInPath == statePath.Count-1)
            {
                currentPositionInPath = 0;
                GesteDetected();
            }
        }
        else if (!statePath[currentPositionInPath + 1].Second)
        {
            currentPositionInPath++;
            if (currentPositionInPath == statePath.Count-1)
            {
                currentPositionInPath = 0;
                GesteDetected();
                return;
            }
            OnStateChange(newState);
            return;
        }
        else
        {
            currentPositionInPath = 0;
        }
    }

    private void GesteDetected()
    {
        Debug.Log("Raise " + mType);
        EventManager.raise<GesteTypes>(MyEventTypes.GESTE_DETECTED, mType);
    }

    protected abstract void OnStart();

    protected Geste()
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
    RUN,
    HI
}
