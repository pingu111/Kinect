using UnityEngine;
using System.Collections;
using System;

public class SwipeGauche : Geste
{
    public SwipeGauche() : base()
    {
    }

    protected override void OnStart()
    {
        mType = GesteTypes.SWIPE_LEFT_WITH_RIGHT_HAND;
        AddStateToPath(CurrentState.IDLE_BODY);
        AddStateToPath(CurrentState.RIGHT_HAND_LEFT);
        AddStateToPath(CurrentState.IDLE_BODY);
    }
}
