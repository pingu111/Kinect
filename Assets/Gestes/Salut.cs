using UnityEngine;
using System.Collections;
using System;

public class Salut : Geste {
    protected override void OnStart()
    {
        mType = GesteTypes.HI;
        AddStateToPath(CurrentState.RIGHT_HAND_ORIENTATION_LEFT);
        AddStateToPath(CurrentState.RIGHT_HAND_ORIENTATION_RIGHT);
    }
}
