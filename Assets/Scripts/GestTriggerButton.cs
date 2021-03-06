﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GestTriggerButton : MonoBehaviour {

    public GameObject rightHand;

    public List<ButtonToChangeScene> buttons;

    public GameObject buttonActuallyTouched;

    private bool touchChanged = true;

    private float timeDuringTouchDoesntChanged = 0;

    private float timeBeforeTouchOK = 1f;

    // Use this for initialization
    void Start()
    {
        buttons = new List<ButtonToChangeScene>(GameObject.FindObjectsOfType<ButtonToChangeScene>());
        EventManager.addActionToEvent<GesteTypes>(MyEventTypes.GESTE_DETECTED,(GesteTypes lol)=> {; } );
    }


    // Update is called once per frame
    void Update ()
    {
        
        if(rightHand != null)
        {
            if(buttonActuallyTouched != null && !isTouchingButton(buttonActuallyTouched, rightHand.transform.position))
                touchChanged = true;

            foreach (ButtonToChangeScene b in buttons)
            {
                if(isTouchingButton(b.gameObject, rightHand.transform.position))
                {
                    b.GetComponentInChildren<TextMesh>().color = Color.green;

                    foreach(SpriteRenderer sr in b.GetComponentsInChildren<SpriteRenderer>())
                    {
                        if (sr.transform.parent == b.transform)
                            sr.gameObject.transform.localScale = new Vector2((1 - Mathf.Min( timeDuringTouchDoesntChanged / timeBeforeTouchOK,1)), sr.gameObject.transform.localScale.y);
                    }

                    if (buttonActuallyTouched != b.gameObject)
                    {
                        touchChanged = true;
                    }

                    buttonActuallyTouched = b.gameObject;
                }
                else
                {
                    b.GetComponentInChildren<TextMesh>().color = Color.black;
                    foreach (SpriteRenderer sr in b.GetComponentsInChildren<SpriteRenderer>())
                    {
                        if (sr.transform.parent == b.transform)
                        {
                            sr.gameObject.transform.localScale = new Vector2(1, sr.gameObject.transform.localScale.y);
                        }
                    }
                }
            }

            if (!touchChanged && buttonActuallyTouched !=null)
            {
                timeDuringTouchDoesntChanged += Time.deltaTime;
                if (timeDuringTouchDoesntChanged > timeBeforeTouchOK)
                    buttonActuallyTouched.GetComponent<ButtonToChangeScene>().callEvent();
            }
            else
            {
                timeDuringTouchDoesntChanged = 0;
                touchChanged = false;
            }
        }
        else
        {
            if (this.gameObject.GetComponent<BodyState>() != null)
                rightHand = this.gameObject.GetComponent<BodyState>().rightHand;
        }
    }

    bool isTouchingButton(GameObject button, Vector2 pos)
    {
        bool touching = false;
        if (    pos.x < (button.transform.position.x + button.GetComponent<SpriteRenderer>().bounds.size.x / 2) 
             && pos.x > (button.transform.position.x - button.GetComponent<SpriteRenderer>().bounds.size.x / 2) 
             && pos.y < (button.transform.position.y + button.GetComponent<SpriteRenderer>().bounds.size.y / 2)
             && pos.y > (button.transform.position.y - button.GetComponent<SpriteRenderer>().bounds.size.y / 2))
            touching = true;

        return touching;
    }

    void OnDestroy()
    {
        EventManager.removeActionFromEvent<GesteTypes>(MyEventTypes.GESTE_DETECTED, (GesteTypes lol) => {; });
    }
}


