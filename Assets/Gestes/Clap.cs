using UnityEngine;
using System.Collections;
using System;

public class Clap : Geste
{

    public Clap() : base()
    {
    }

    protected override void OnStart()
    {
        mType = GesteTypes.CLAP;
        AddStateToPath(CurrentState.IDLE_BODY);
        AddStateToPath(CurrentState.CLAP_HAND);
    }
}
