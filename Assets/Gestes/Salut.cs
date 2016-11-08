using UnityEngine;
using System.Collections;
using System;

public class Salut : Geste {

    public Salut() : base()
    {
    }

    protected override void OnStart()
    {
        ignoreHandOrientation = false;
        mType = GesteTypes.HI;
        AddStateToPath(CurrentState.IDLE_HAND);
        AddStateToPath(CurrentState.RIGHT_HAND_ORIENTATION_LEFT,false);
        AddStateToPath(CurrentState.IDLE_HAND,false);
        AddStateToPath(CurrentState.RIGHT_HAND_ORIENTATION_RIGHT);
        AddStateToPath(CurrentState.IDLE_HAND, false);
        AddStateToPath(CurrentState.RIGHT_HAND_ORIENTATION_LEFT);
        AddStateToPath(CurrentState.IDLE_HAND);

    }
}
