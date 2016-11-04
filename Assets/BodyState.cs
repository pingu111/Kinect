using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;

public class BodyState : MonoBehaviour
{
    // Les parties du corps qu'on suit

    public GameObject rightHand;

    public GameObject leftHand;

    public GameObject leftWrist;

    public GameObject rightWrist;

    public GameObject rightShoulder;

    public GameObject leftShoulder;

    public GameObject middleBody;

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
        listGestes = new List<Geste>{new ParlesAMaMain(), new PeauDeLapin(), new Run(), new Salut(), new SwipeDroite(), new SwipeGauche()};
        CurrentHandOr = CurrentState.IDLE_HAND;
        CurrentStateBody = CurrentState.IDLE_BODY;
    }

	// Update is called once per frame
	void Update ()
    {
	    if(rightHand != null && leftHand != null && rightShoulder != null && leftShoulder != null && middleBody != null)
        {
            // State of the positions
            if (distanceShoulders > 4)
            {
                CurrentState previewState = CurrentStateBody;

                if (isRightHandRight())
                {
                    previewState = CurrentState.RIGHT_HAND_RIGHT;
                }
                else if (isRightHandLeft())
                {
                    previewState = CurrentState.RIGHT_HAND_LEFT;
                }
                else if (isLeftHandUp() && isRightHandUp())
                {
                    previewState = CurrentState.HANDSUP;
                }
                else if (isRightHandFront())
                {
                    previewState = CurrentState.RIGHT_HAND_FRONT;
                }
                else if (isLeftHandFront())
                {
                    previewState = CurrentState.LEFT_HAND_FRONT;
                }
                else
                {
                    previewState = CurrentState.IDLE_BODY;
                    if (Vector3.Distance(rightShoulder.transform.position, leftShoulder.transform.position) > 0)
                    {
                        distanceShoulders = Mathf.Abs(rightShoulder.transform.position.x- leftShoulder.transform.position.x);
                        /*nbMeasuresShoulders++;
                        distanceShoulders = (distanceShoulders * (nbMeasuresShoulders - 1) + Vector3.Distance(rightShoulder.transform.position, leftShoulder.transform.position)) / (nbMeasuresShoulders);*/
                    }
                }

                if (previewState != CurrentStateBody)
                {
                    Debug.Log(CurrentStateBody+" to "+previewState);
                    CurrentStateBody = previewState;
                    EventManager.raise(MyEventTypes.STATE_CHANGED, CurrentStateBody);
                }

                //  State of orientations
                CurrentState previewOrientationState = CurrentHandOr;
            }
            else
            {
                if (Vector3.Distance(rightShoulder.transform.position, leftShoulder.transform.position) > 0)
                {
                    distanceShoulders = Mathf.Abs(rightShoulder.transform.position.x - leftShoulder.transform.position.x);
                    /*nbMeasuresShoulders++;
                    distanceShoulders = (distanceShoulders * (nbMeasuresShoulders - 1) + Vector3.Distance(rightShoulder.transform.position, leftShoulder.transform.position)) / (nbMeasuresShoulders);*/
                }
            }
        }
	}


    bool isRightHandRight()
    {
        return(rightHand.transform.position.x > rightShoulder.transform.position.x + distanceShoulders);
    }

    bool isRightHandLeft()
    {
        return (rightHand.transform.position.x < leftShoulder.transform.position.x);
    }

    bool isLeftHandFront()
    {
        return (leftHand.transform.position.z < middleBody.transform.position.z - distanceShoulders*1.0f);
    }

    bool isRightHandFront()
    {
        return (rightHand.transform.position.z < middleBody.transform.position.z - distanceShoulders * 1.0f);
    }

    bool isRightHandUp()
    {
        return (rightHand.transform.position.y > rightShoulder.transform.position.y);
    }

    bool isLeftHandUp()
    {
        return (leftHand.transform.position.y > leftShoulder.transform.position.y);
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

    [TypeOfStateValue(TypeOfState.HAND_ORIENTATION)]
    IDLE_HAND,
    [TypeOfStateValue(TypeOfState.HAND_ORIENTATION)]
    RIGHT_HAND_ORIENTATION_LEFT,
    [TypeOfStateValue(TypeOfState.HAND_ORIENTATION)]
    RIGHT_HAND_ORIENTATION_RIGHT
}

