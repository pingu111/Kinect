using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GesteText : MonoBehaviour {

    void Start()
    {
        this.gameObject.GetComponent<Text>().text = "";
        EventManager.addActionToEvent<GesteTypes>(MyEventTypes.GESTE_DETECTED, gesteDetected);
    }

    void gesteDetected(GesteTypes _type)
    {
        string newText = "";
        switch(_type)
        {
            case GesteTypes.HANDS_UP:
                newText = "Mains en haut ";
                break;
            case GesteTypes.HI:
                newText = "Bonjour ";
                break;
            case GesteTypes.RUN:
                newText = "Course ";
                break;
            case GesteTypes.SPEAK_TO_THE_HAND:
                newText = "Main devant ";
                break;
            case GesteTypes.SWIPE_LEFT_WITH_RIGHT_HAND:
                newText = "Baffe vers la gauche ";
                break;
            case GesteTypes.SWIPE_RIGHT_WITH_RIGHT_HAND:
                newText = "Baffe vers la droite ";
                break;
            default:
                break;
        }
        this.gameObject.GetComponent<Text>().text = (newText + " Detecté !");
    }
}
