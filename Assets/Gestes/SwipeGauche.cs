using UnityEngine;
using System.Collections;
using System;

public class SwipeGauche : Geste
{
    protected override void OnStart()
    {
        mType = GesteTypes.SWIPE_LEFT_WITH_RIGHT_HAND;
        statePath.Add(CurrentState.IDLE);
        statePath.Add(CurrentState.RIGHT_HAND_LEFT);
        statePath.Add(CurrentState.IDLE);
    }
}
