using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ParlesAMaMain : Geste
{ 
    protected override void OnStart ()
    {
        mType = GesteTypes.SPEAK_TO_THE_HAND;
        AddStateToPath(CurrentState.IDLE_BODY);
        AddStateToPath(CurrentState.RIGHT_HAND_FRONT);
        AddStateToPath(CurrentState.IDLE_BODY);
    }
}
