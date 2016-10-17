using UnityEngine;
using System.Collections;
using System;

public class PeauDeLapin : Geste
{
    protected override void OnStart()
    {
        mType = GesteTypes.HANDS_UP;
        statePath.Add(CurrentState.IDLE_BODY);
        statePath.Add(CurrentState.HANDSUP);
        statePath.Add(CurrentState.IDLE_BODY);
    }
}
