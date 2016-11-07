using UnityEngine;
using System.Collections;
using System;

public class PeauDeLapin : Geste
{
    public PeauDeLapin() : base()
    {
    }

    protected override void OnStart()
    {
        mType = GesteTypes.HANDS_UP;
        AddStateToPath(CurrentState.IDLE_BODY);
        AddStateToPath(CurrentState.LEFT_HAND_FRONT, false);
        AddStateToPath(CurrentState.RIGHT_HAND_FRONT, false);
        AddStateToPath(CurrentState.LEFT_HAND_FRONT, false);
        AddStateToPath(CurrentState.HANDSUP);
        AddStateToPath(CurrentState.LEFT_HAND_FRONT, false);
        AddStateToPath(CurrentState.RIGHT_HAND_FRONT, false);
        AddStateToPath(CurrentState.LEFT_HAND_FRONT, false);
        AddStateToPath(CurrentState.IDLE_BODY);

    }
}
