using UnityEngine;
using System.Collections;
using System;

public class Run : Geste
{
    public Run() : base()
    {
    }

    protected override void OnStart()
    {
        mType = GesteTypes.RUN;
        AddStateToPath(CurrentState.IDLE_BODY);

        AddStateToPath(CurrentState.RIGHT_HAND_FRONT, false);
        AddStateToPath(CurrentState.RIGHT_HAND_RUNNING,false);
        AddStateToPath(CurrentState.RIGHT_HAND_FRONT, false);

        AddStateToPath(CurrentState.IDLE_BODY, false);

        AddStateToPath(CurrentState.LEFT_HAND_FRONT, false);
        AddStateToPath(CurrentState.LEFT_HAND_RUNNING);
        AddStateToPath(CurrentState.LEFT_HAND_FRONT, false);

        AddStateToPath(CurrentState.IDLE_BODY, false);

        AddStateToPath(CurrentState.RIGHT_HAND_FRONT, false);
        AddStateToPath(CurrentState.RIGHT_HAND_RUNNING);
        AddStateToPath(CurrentState.RIGHT_HAND_FRONT, false);

        AddStateToPath(CurrentState.IDLE_BODY, false);

        AddStateToPath(CurrentState.LEFT_HAND_FRONT, false);
        AddStateToPath(CurrentState.LEFT_HAND_RUNNING, false);
        AddStateToPath(CurrentState.LEFT_HAND_FRONT, false);

        AddStateToPath(CurrentState.IDLE_BODY);
    }
}
