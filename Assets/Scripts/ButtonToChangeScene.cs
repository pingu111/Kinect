using UnityEngine;
using System.Collections;

public class ButtonToChangeScene : MonoBehaviour
{

    public MyEventTypes eventToCallIfClicked;

    public ScenesType typeOfSceneChanging;
    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePoss = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (     mousePoss.x < (this.gameObject.transform.position.x + this.gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 2)
                  && mousePoss.x > (this.gameObject.transform.position.x - this.gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 2)
                  && mousePoss.y < (this.gameObject.transform.position.y + this.gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2)
                  && mousePoss.y > (this.gameObject.transform.position.y - this.gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2))
            {
                callEvent();
            }
        }

    }

    public void callEvent()
    {
        Debug.Log(eventToCallIfClicked);
        if(eventToCallIfClicked == MyEventTypes.CHANGE_SCENE)
            EventManager.raise<ScenesType>(eventToCallIfClicked, typeOfSceneChanging);
    }
}
