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
        AddStateToPath(CurrentState.HANDSUP);
        AddStateToPath(CurrentState.IDLE_BODY);
    }
}
