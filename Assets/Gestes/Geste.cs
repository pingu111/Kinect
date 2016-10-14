using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Geste
{
    private CurrentState triggerState;
    private List<CurrentState> statePath;

    public abstract void OnStateChange();

    public abstract void GesteDetected();
}
