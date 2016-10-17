using UnityEngine;
using System.Collections;
using System;

public class SwipeDroite : Geste
{
    protected override void OnStart()
    {
        mType = GesteTypes.SWIPE_RIGHT_WITH_RIGHT_HAND;
        statePath.Add(CurrentState.IDLE_BODY);
        statePath.Add(CurrentState.RIGHT_HAND_RIGHT);
        statePath.Add(CurrentState.IDLE_BODY);
    }
}
