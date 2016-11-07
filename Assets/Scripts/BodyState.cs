using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;

public class BodyState : MonoBehaviour
{

    public GameObject knobIfDetected;


    // Les parties du corps qu'on suit
    public GameObject masterHand;

    public GameObject otherHand;

    public GameObject rightWrist;

    public GameObject masterShoulder;

    public GameObject otherShoulder;

    public GameObject middleBody;

    public GameObject rightTip;

    private float threesholdHandOr = 20;

    private CurrentState _currentHandOr;
    private CurrentState CurrentHandOr
    {
        get { return _currentHandOr; }
        set
        {
            if(TypoeOfStateEnum.GetTypoeOfStateValue(value).Equals(TypeOfState.HAND_ORIENTATION))
                _currentHandOr = value;
        }
    }

    private CurrentState _currentStateBody;
    private CurrentState CurrentStateBody
    {
        get { return _currentStateBody; }
        set
        {
            if (TypoeOfStateEnum.GetTypoeOfStateValue(value).Equals(TypeOfState.BODY_STATE))
                _currentStateBody = value;
        }
    }
    private bool measureHasBeenDone = false;
    private float distanceShoulders = 0;
    private int nbMeasuresShoulders = 0;


    public List<Geste> listGestes;


    void Start()
    {
        listGestes = new List<Geste>{new ParlesAMaMain(), new PeauDeLapin(), new Run(), new Salut(), new SwipeDroite(), new SwipeGauche(), new Clap()};
        CurrentHandOr = CurrentState.IDLE_HAND;
        CurrentStateBody = CurrentState.IDLE_BODY;
    }

	// Update is called once per frame
	void Update ()
    {
	    if(masterHand != null && otherHand != null && masterShoulder != null && otherShoulder != null && middleBody != null)
        {
            // State of the positions
            if (distanceShoulders > 2.5f)
            {
                if(knobIfDetected != null)
                    knobIfDetected.SetActive(false);

                CurrentState previewState = CurrentStateBody;

                if (isLeftHandUp() && isRightHandUp())
                {
                    previewState = CurrentState.HANDSUP;
                }
                else if (isRightHandRight())
                {
                    previewState = CurrentState.RIGHT_HAND_RIGHT;
                }
                else if (isRightHandLeft())
                {
                    previewState = CurrentState.RIGHT_HAND_LEFT;
                }
                else if (isRightHandFront())
                {
                    previewState = CurrentState.RIGHT_HAND_FRONT;
                }
                else if (isLeftHandFront())
                {
                    previewState = CurrentState.LEFT_HAND_FRONT;
                }
                else if(areHandClapped())
                    previewState = CurrentState.CLAP_HAND;
                else
                {
                    previewState = CurrentState.IDLE_BODY;
                    if (Vector3.Distance(masterShoulder.transform.position, otherShoulder.transform.position) > 0)
                    {
                        distanceShoulders = Mathf.Abs(masterShoulder.transform.position.x - otherShoulder.transform.position.x);
                        /*nbMeasuresShoulders++;
                        distanceShoulders = (distanceShoulders * (nbMeasuresShoulders - 1) + Vector3.Distance(rightShoulder.transform.position, leftShoulder.transform.position)) / (nbMeasuresShoulders);*/
                    }
                }

                if (previewState != CurrentStateBody)
                {
                    Debug.Log("new :"+previewState);
                    CurrentStateBody = previewState;
                    EventManager.raise(MyEventTypes.STATE_CHANGED, CurrentStateBody);
                }


                //  State of orientations
                previewState = CurrentHandOr;

                if (isRightHandOrientedLeft())
                {
                    previewState = CurrentState.RIGHT_HAND_ORIENTATION_LEFT;
                }
                else if (isRightHandOrientedRight())
                {
                    previewState = CurrentState.RIGHT_HAND_ORIENTATION_RIGHT;
                }
                else if (isRightHandIdle())
                {
                    previewState = CurrentState.IDLE_HAND;
                }

                if (previewState != CurrentHandOr)
                {
                    Debug.Log("new :" + previewState);
                    CurrentHandOr = previewState;
                    EventManager.raise(MyEventTypes.STATE_CHANGED, CurrentHandOr);
                }

            }
            else
            {
                if (knobIfDetected != null)
                    knobIfDetected.SetActive(true);

                if (Vector3.Distance(masterShoulder.transform.position, otherShoulder.transform.position) > 0)
                {
                    distanceShoulders = Mathf.Abs(masterShoulder.transform.position.x - otherShoulder.transform.position.x);
                    /*nbMeasuresShoulders++;
                    distanceShoulders = (distanceShoulders * (nbMeasuresShoulders - 1) + Vector3.Distance(rightShoulder.transform.position, leftShoulder.transform.position)) / (nbMeasuresShoulders);*/
                }
            }
        }
	}


    bool isRightHandRight()
    {
        return(masterHand.transform.position.x > masterShoulder.transform.position.x + distanceShoulders *1.0f);
    }

    bool isRightHandLeft()
    {
        return (masterHand.transform.position.x < otherShoulder.transform.position.x);
    }

    bool isLeftHandFront()
    {
        return (otherHand.transform.position.z < otherShoulder.transform.position.z - distanceShoulders*1.0f);
    }

    bool isRightHandFront()
    {
        return (masterHand.transform.position.z < masterShoulder.transform.position.z - distanceShoulders * 1.0f);
    }

    bool isRightHandUp()
    {
        return (masterHand.transform.position.y > masterShoulder.transform.position.y);
    }

    bool isLeftHandUp()
    {
        return (otherHand.transform.position.y > otherShoulder.transform.position.y);
    }

    bool isRightHandOrientedRight()
    {
        float dx = rightTip.transform.position.x - rightWrist.transform.position.x;
        float dy = rightTip.transform.position.y - rightWrist.transform.position.y;
        return (Mathf.Atan2(dx, dy) * Mathf.Rad2Deg > threesholdHandOr);
    }
    bool isRightHandOrientedLeft()
    {
        float dx = rightTip.transform.position.x - rightWrist.transform.position.x;
        float dy = rightTip.transform.position.y - rightWrist.transform.position.y;
        return (Mathf.Atan2(dx, dy) * Mathf.Rad2Deg < -threesholdHandOr);
    }

    bool isRightHandIdle()
    {
        float dx = rightTip.transform.position.x - rightWrist.transform.position.x;
        float dy = rightTip.transform.position.y - rightWrist.transform.position.y;
        return (Mathf.Atan2(dx, dy) * Mathf.Rad2Deg > -threesholdHandOr && Mathf.Atan2(dx, dy) * Mathf.Rad2Deg < threesholdHandOr);
    }


    bool areHandClapped()
    {
        return (Vector3.Distance(masterHand.transform.position, otherHand.transform.position) < 1);
    }

}



public class TypeOfStateValue : System.Attribute
{
    private readonly TypeOfState _value;

    public TypeOfStateValue(TypeOfState value)
    {
        _value = value;
    }

    public TypeOfState value
    {
        get { return _value; }
    }

}


public static class TypoeOfStateEnum
{
    public static TypeOfState GetTypoeOfStateValue(Enum value)
    {
        TypeOfState output = 0;
        Type type = value.GetType();

        FieldInfo fi = type.GetField(value.ToString());

        TypeOfStateValue[] attrs =
           fi.GetCustomAttributes(typeof(TypeOfStateValue),
                                   false) as TypeOfStateValue[];
        if (attrs.Length > 0)
        {
            output = attrs[0].value;
        }

        return output;
    }
}

public enum TypeOfState
{
    HAND_ORIENTATION,
    BODY_STATE
}

public enum CurrentState
{
    [TypeOfStateValue(TypeOfState.BODY_STATE)]
    IDLE_BODY,
    [TypeOfStateValue(TypeOfState.BODY_STATE)]
    RIGHT_HAND_FRONT,
    [TypeOfStateValue(TypeOfState.BODY_STATE)]
    LEFT_HAND_FRONT,
    [TypeOfStateValue(TypeOfState.BODY_STATE)]
    RIGHT_HAND_RIGHT,
    [TypeOfStateValue(TypeOfState.BODY_STATE)]
    RIGHT_HAND_LEFT,
    [TypeOfStateValue(TypeOfState.BODY_STATE)]
    HANDSUP,
    [TypeOfStateValue(TypeOfState.BODY_STATE)]
    CLAP_HAND,

    [TypeOfStateValue(TypeOfState.HAND_ORIENTATION)]
    IDLE_HAND,
    [TypeOfStateValue(TypeOfState.HAND_ORIENTATION)]
    RIGHT_HAND_ORIENTATION_LEFT,
    [TypeOfStateValue(TypeOfState.HAND_ORIENTATION)]
    RIGHT_HAND_ORIENTATION_RIGHT
}

