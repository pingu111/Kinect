using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ParlesAMaMain : Geste
{ 
    protected override void OnStart ()
    {
        mType = GesteTypes.SPEAK_TO_THE_HAND;
        statePath.Add(CurrentState.IDLE);
        statePath.Add(CurrentState.RIGHT_HAND_FRONT);
        statePath.Add(CurrentState.IDLE);
    }
}
