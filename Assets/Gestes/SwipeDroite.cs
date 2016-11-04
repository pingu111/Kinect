using UnityEngine;
using System.Collections;
using System;

public class SwipeDroite : Geste
{
    protected override void OnStart()
    {
        mType = GesteTypes.SWIPE_RIGHT_WITH_RIGHT_HAND;
        AddStateToPath(CurrentState.IDLE_BODY);
        AddStateToPath(CurrentState.RIGHT_HAND_RIGHT);
        AddStateToPath(CurrentState.IDLE_BODY);
    }
}
