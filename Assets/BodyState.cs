using UnityEngine;
using System.Collections;

public class BodyState : MonoBehaviour
{
    // Les parties du corps qu'on suit

    public GameObject rightHand;

    public GameObject leftHand;

    public GameObject rightShoulder;

    public GameObject leftShoulder;

    public GameObject middleBody;

    private CurrentHandOrientation currentHandOr;

    private CurrentState currentState;

	// Update is called once per frame
	void Update ()
    {
	    if(rightHand != null && leftHand != null && rightShoulder != null && leftShoulder != null && middleBody != null)
        {

        }
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
    RIGHT_HAND_ORIENTATION_LEFT,
    RIGHT_HAND_ORIENTATION_RIGHT
}