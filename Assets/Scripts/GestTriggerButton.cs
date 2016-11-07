using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GestTriggerButton : MonoBehaviour {

    public GameObject rightHand;

    public List<Button> buttons;

    public GameObject buttonActuallyTouched;

    private bool touchChanged = false;

    private float timeDuringTouchDoesntChanged = 0;

    private float timeBeforeTouchOK = 1f;


	// Use this for initialization
	void Start ()
    {
        buttons = new List<Button>(GameObject.FindObjectsOfType<Button>());
    }


    // Update is called once per frame
    void Update ()
    {
        if(rightHand != null)
        {
            if(!isTouchingButton(buttonActuallyTouched, rightHand.transform.position))
                touchChanged = true;

            foreach (Button b in buttons)
            {
                if(isTouchingButton(b.gameObject, rightHand.transform.position))
                {
                    if (buttonActuallyTouched != b.gameObject)
                        touchChanged = true;

                    buttonActuallyTouched = b.gameObject;
                }
            }

            if(!touchChanged)
            {
                timeDuringTouchDoesntChanged += Time.deltaTime;
                if (timeDuringTouchDoesntChanged > timeBeforeTouchOK)
                    buttonActuallyTouched.GetComponent<Button>().onClick.Invoke();
            }
            else
            {
                timeDuringTouchDoesntChanged = 0;
                touchChanged = false;
            }
        }
    }

    bool isTouchingButton(GameObject button, Vector2 pos)
    {
        bool touching = false;
        if (    pos.x < (button.transform.position.x + button.GetComponent<RectTransform>().rect.height / 2) 
             && pos.x > (button.transform.position.x - button.GetComponent<RectTransform>().rect.width / 2) 
             && pos.y < (button.transform.position.y + button.GetComponent<RectTransform>().rect.height / 2)
             && pos.y > (button.transform.position.y - button.GetComponent<RectTransform>().rect.height / 2))
            touching = true;
        return touching;
    }
}


