using UnityEngine;
using System.Collections;
using System;

public class Run : Geste
{
    protected override void OnStart()
    {
        mType = GesteTypes.RUN;
        AddStateToPath(CurrentState.IDLE_BODY);
        AddStateToPath(CurrentState.LEFT_HAND_FRONT);
        AddStateToPath(CurrentState.RIGHT_HAND_FRONT);
        AddStateToPath(CurrentState.LEFT_HAND_FRONT);
    }
}
