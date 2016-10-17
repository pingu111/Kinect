using UnityEngine;
using System.Collections;
using System;

public class Salut : Geste {
    protected override void OnStart()
    {
        mType = GesteTypes.HI;
        statePath.Add(CurrentState.RIGHT_HAND_ORIENTATION_LEFT);
        statePath.Add(CurrentState.RIGHT_HAND_ORIENTATION_RIGHT);
    }
}
