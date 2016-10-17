using UnityEngine;
using System.Collections;
using System;

public class Run : Geste
{
    protected override void OnStart()
    {
        throw new NotImplementedException();
        mType = GesteTypes.RUN;
        statePath.Add(CurrentState.IDLE_BODY);
        statePath.Add(CurrentState.HANDSUP);
        statePath.Add(CurrentState.IDLE_BODY);
    }
}
