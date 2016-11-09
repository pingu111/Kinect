using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// pour la creation de tout nouveaux gestes :
/// creer la méthode OnStart et y informer le type dans mType 
/// y ajouter ignoreHandOrientation si on veut ecouter l'orientation de la main
/// puis ajouter les differents etats avec AddStateToPath (informer si l'est est important ou non avec le booleen)
/// 
/// IMPORTANT toujours commencer par un IDLE_BODY !
/// </summary>
public abstract class Geste
{
    protected bool ignoreHandOrientation = true;
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
        if (TypoeOfStateEnum.GetTypoeOfStateValue(newState).Equals(TypeOfState.HAND_ORIENTATION) && ignoreHandOrientation)
            return;
        if (newState == statePath[currentPositionInPath + 1].First)
        {
            currentPositionInPath++;
            if (currentPositionInPath == statePath.Count-1)
            {
                currentPositionInPath = -1;
                GesteDetected();
                OnStateChange(newState);
                return;
            }
        }
        else if (!statePath[currentPositionInPath + 1].Second)
        {
            currentPositionInPath++;
            if (currentPositionInPath == statePath.Count-1)
            {
                currentPositionInPath = -1;
                GesteDetected();
                OnStateChange(newState);
                return;
            }
            OnStateChange(newState);
            return;
        }
        else
        {
            currentPositionInPath = -1;
            if (newState.Equals(CurrentState.IDLE_BODY))
                currentPositionInPath++;
        }
    }

    private void GesteDetected()
    {
        Debug.Log("/************ RAISE " + mType+" ***********/ "+Time.time);
        EventManager.raise<GesteTypes>(MyEventTypes.GESTE_DETECTED, mType);
    }

    protected abstract void OnStart();

    protected Geste()
    {
        EventManager.addActionToEvent<CurrentState>(MyEventTypes.STATE_CHANGED, OnStateChange);
        OnStart();
    }

    public void OnDestroy()
    {
        EventManager.removeActionFromEvent<CurrentState>(MyEventTypes.STATE_CHANGED, OnStateChange);
    }

    public void reset()
    {
        currentPositionInPath = -1;
    }
}



public enum GesteTypes
{
    SPEAK_TO_THE_HAND,
    HANDS_UP,
    CLAP,
    SWIPE_LEFT_WITH_RIGHT_HAND,
    SWIPE_RIGHT_WITH_RIGHT_HAND,
    RUN,
    HI,
    NO_GESTES_TIMER
}
