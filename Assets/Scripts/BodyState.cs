using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;

public class BodyState : MonoBehaviour
{

    public GameObject knobIfDetected;


    // Les parties du corps qu'on suit
    public GameObject rightHand;
    public GameObject leftHand;

    public GameObject rightWrist;
    public GameObject leftWrist;

    public GameObject rightShoulder;
    public GameObject leftShoulder;

    public GameObject middleBody;

    public GameObject rightTip;
    public GameObject leftTip;


    private float threesholdHandOr = 30;

    int nbGest = 0;

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

    public bool isRightHandMaster = true;

    public List<Geste> listGestes = new List<Geste>();


    void Start()
    {
        listGestes.Clear();
        listGestes = new List<Geste>{new ParlesAMaMain(), new PeauDeLapin(), new Run(), new Salut(), new SwipeDroite(), new SwipeGauche(), new Clap()};

        CurrentHandOr = CurrentState.IDLE_HAND;
        CurrentStateBody = CurrentState.IDLE_BODY;

        if (FindObjectsOfType<GetInformationsScript>().Length > 0)
            isRightHandMaster = FindObjectOfType<GetInformationsScript>().userInfos.isRightHanded;
    }

	// Update is called once per frame
	void Update ()
    {
	    if(rightHand != null && leftHand != null && rightShoulder != null && leftShoulder != null && middleBody != null)
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
                else if(isRightHandRunning())
                {
                    previewState = CurrentState.RIGHT_HAND_RUNNING;
                }
                else if (isLeftHandRunning())
                {
                    previewState = CurrentState.LEFT_HAND_RUNNING;
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
                    if (Vector3.Distance(rightShoulder.transform.position, leftShoulder.transform.position) > 0)
                    {
                        distanceShoulders = Mathf.Abs(rightShoulder.transform.position.x - leftShoulder.transform.position.x);
                    }
                }

                if (previewState != CurrentStateBody)
                {
                    Debug.Log("new :"+previewState+" "+Time.time +" "+ nbGest);
                    nbGest++;
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
                   // Debug.Log("new :" + previewState);
                    CurrentHandOr = previewState;
                    EventManager.raise(MyEventTypes.STATE_CHANGED, CurrentHandOr);
                }
            }
            else
            {
                if (knobIfDetected != null)
                    knobIfDetected.SetActive(true);

                if (Vector3.Distance(rightShoulder.transform.position, leftShoulder.transform.position) > 0)
                {
                    distanceShoulders = Mathf.Abs(rightShoulder.transform.position.x - leftShoulder.transform.position.x);                 
                }
            }
        }
	}


    bool isRightHandRight()
    {
        return(rightHand.transform.position.x > rightShoulder.transform.position.x + distanceShoulders * 0.75f);
    }
    bool isLeftHandRight()
    {
        return (leftHand.transform.position.x < leftShoulder.transform.position.x - distanceShoulders * 0.75f);
    }


    bool isRightHandLeft()
    {
        return (rightHand.transform.position.x < leftShoulder.transform.position.x);
    }
    bool isLeftHandLeft()
    {
        return (leftHand.transform.position.x > leftShoulder.transform.position.x);
    }

    bool isLeftHandRunning()
    {
        return ((leftHand.transform.position.z < leftShoulder.transform.position.z + distanceShoulders * 0.25f) &&
            (Mathf.Abs(leftHand.transform.position.y - leftShoulder.transform.position.y) < 0.5f) &&
             leftHand.transform.position.x > leftShoulder.transform.position.x &&
             leftHand.transform.position.x < rightShoulder.transform.position.x) ; 
    }
    bool isRightHandRunning()
    {
        return ((rightHand.transform.position.z < rightShoulder.transform.position.z + distanceShoulders * 0.25f) && 
            (Mathf.Abs(rightHand.transform.position.y - rightShoulder.transform.position.y) < 0.5f) &&
            rightHand.transform.position.x > leftShoulder.transform.position.x &&
            rightHand.transform.position.x < rightShoulder.transform.position.x);
    }


    bool isLeftHandFront()
    {
        return (leftHand.transform.position.z < leftShoulder.transform.position.z - distanceShoulders*1.5f);
    }
    bool isRightHandFront()
    {
        return (rightHand.transform.position.z < rightShoulder.transform.position.z - distanceShoulders * 1.5f);
    }


    bool isRightHandUp()
    {
        return (rightHand.transform.position.y > rightShoulder.transform.position.y);
    }
    bool isLeftHandUp()
    {
        return (leftHand.transform.position.y > leftShoulder.transform.position.y);
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


    bool isLeftHandOrientedRight()
    {
        float dx = leftTip.transform.position.x - leftWrist.transform.position.x;
        float dy = leftTip.transform.position.y - leftWrist.transform.position.y;
        return (Mathf.Atan2(dx, dy) * Mathf.Rad2Deg > threesholdHandOr);
    }
    bool isLeftHandOrientedLeft()
    {
        float dx = leftTip.transform.position.x - leftWrist.transform.position.x;
        float dy = leftTip.transform.position.y - leftWrist.transform.position.y;
        return (Mathf.Atan2(dx, dy) * Mathf.Rad2Deg < -threesholdHandOr);
    }

    bool isLeftHandIdle()
    {
        float dx = leftTip.transform.position.x - leftWrist.transform.position.x;
        float dy = leftTip.transform.position.y - leftWrist.transform.position.y;
        return (Mathf.Atan2(dx, dy) * Mathf.Rad2Deg > -threesholdHandOr && Mathf.Atan2(dx, dy) * Mathf.Rad2Deg < threesholdHandOr);
    }


    bool areHandClapped()
    {
        return (Vector3.Distance(rightHand.transform.position, leftHand.transform.position) < 1.0f &&
            Mathf.Abs(rightHand.transform.position.y - rightShoulder.transform.position.y) < 1.0f );
    }

    void OnDestroy()
    {
        foreach (Geste g in listGestes)
        {
            g.OnDestroy();
        }
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
    [TypeOfStateValue(TypeOfState.BODY_STATE)]
    LEFT_HAND_RUNNING,
    [TypeOfStateValue(TypeOfState.BODY_STATE)]
    RIGHT_HAND_RUNNING,

    [TypeOfStateValue(TypeOfState.HAND_ORIENTATION)]
    IDLE_HAND,
    [TypeOfStateValue(TypeOfState.HAND_ORIENTATION)]
    RIGHT_HAND_ORIENTATION_LEFT,
    [TypeOfStateValue(TypeOfState.HAND_ORIENTATION)]
    RIGHT_HAND_ORIENTATION_RIGHT
}

