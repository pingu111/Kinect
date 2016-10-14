using UnityEngine;
using System.Collections;

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

    private CurrentHandOrientation currentHandOr;

    private CurrentState currentState;


    private float distanceShoulders;
    private int nbMeasuresShoulders = 0;

    void Start()
    {
        currentHandOr = CurrentHandOrientation.IDLE;
        currentState = CurrentState.IDLE;
    }

	// Update is called once per frame
	void Update ()
    {
	    if(rightHand != null && leftHand != null && rightShoulder != null && leftShoulder != null && middleBody != null)
        {
            // We increase the number of measures of this distance
            nbMeasuresShoulders++;
            distanceShoulders = (distanceShoulders*(nbMeasuresShoulders - 1)+  Vector3.Distance(rightShoulder.transform.position, leftShoulder.transform.position))/ (nbMeasuresShoulders);


            // State of the positions
            CurrentState previewState = currentState;

            if (isRightHandRight())
                previewState = CurrentState.RIGHT_HAND_RIGHT;
            else if (isRightHandLeft())
                previewState = CurrentState.RIGHT_HAND_LEFT;
            else if (isLeftHandUp() && isRightHandUp())
                previewState = CurrentState.HANDSUP;
            else if (isRightHandFront())
                previewState = CurrentState.RIGHT_HAND_FRONT;
            else if (isLeftHandFront())
                previewState = CurrentState.LEFT_HAND_FRONT;
            else
                previewState = CurrentState.IDLE;

            if(previewState != currentState)
            {
                currentState = previewState;
                EventManager.raise(MyEventTypes.STATE_CHANGED, currentState);
            }

            //
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
        return (leftHand.transform.position.z < middleBody.transform.position.z - distanceShoulders*0.5f);
    }

    bool isRightHandFront()
    {
        return (rightHand.transform.position.z < middleBody.transform.position.z - distanceShoulders * 0.5f);
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

public enum CurrentState
{
    IDLE,

    RIGHT_HAND_FRONT,
    LEFT_HAND_FRONT,

    RIGHT_HAND_RIGHT,
    RIGHT_HAND_LEFT,

    HANDSUP
}

public enum CurrentHandOrientation
{
    IDLE,
    RIGHT_HAND_ORIENTATION_LEFT,
    RIGHT_HAND_ORIENTATION_RIGHT
}