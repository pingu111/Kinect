using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class RandomGest : MonoBehaviour {

    public int idGestActuallyAsked;
    private int numberLoop = 0;
    private int nbLoopsToDo = 6;
    public List<GesteTypes> listGestsToDo = new List<GesteTypes>();

    private float timeToDetect = 10.0f;
    private float timeActual = 0;

	// Use this for initialization
	void Start ()
    {
        idGestActuallyAsked = 0;
        createListRandomGeste();
        EventManager.addActionToEvent<GesteTypes>(MyEventTypes.GESTE_DETECTED, gesteDetected);
        askForGest(idGestActuallyAsked);
    }

    void Update()
    {
        timeActual += Time.deltaTime;
        if(timeActual > timeToDetect)
        {
            askForNewGeste();
        }
    }

    void askForNewGeste()
    {
        timeActual = 0;
        idGestActuallyAsked++;
        if (idGestActuallyAsked == listGestsToDo.Count)
        {
            numberLoop++;
            idGestActuallyAsked = 0;
            if (numberLoop > nbLoopsToDo)
            {
                stockerInfos();
            }
        }
        askForGest(idGestActuallyAsked);
    }

    void gesteDetected(GesteTypes _type)
    {
        if (_type == listGestsToDo[idGestActuallyAsked])
        {
            askForNewGeste();
        }
        else
        {
            Debug.Log("Mauvais geste " + _type + " au lieu de " + listGestsToDo[idGestActuallyAsked]);
        }
    }

    void askForGest(int id)
    {
        GesteTypes newGeste = listGestsToDo[id];
        string newText = "";

        switch (newGeste)
        {
            case GesteTypes.HANDS_UP:
                newText = "Haut les mains !";
                break;
            case GesteTypes.HI:
                newText = "Faites bonjour de la main droite ";
                break;
            case GesteTypes.RUN:
                newText = "Courez avec les mains ";
                break;
            case GesteTypes.SPEAK_TO_THE_HAND:
                newText = "Main droite devant vous";
                break;
            case GesteTypes.SWIPE_LEFT_WITH_RIGHT_HAND:
                newText = "Baffe de la main droite vers la gauche ";
                break;
            case GesteTypes.SWIPE_RIGHT_WITH_RIGHT_HAND:
                newText = "Baffe de la main droite vers la droite ";
                break;
            default:
                break;
        }

        this.gameObject.GetComponent<Text>().text = newText;
    }

    private void createListRandomGeste()
    {
        Array values = Enum.GetValues(typeof(GesteTypes));

        // remove of the clap
        while(values.Length - 1 != listGestsToDo.Count)
        {
            System.Random random = new System.Random();
            GesteTypes randomValue = (GesteTypes)values.GetValue(random.Next(values.Length));
            if (!listGestsToDo.Contains(randomValue) && randomValue != GesteTypes.CLAP)
                listGestsToDo.Add(randomValue);
        }
    }

    public void stockerInfos()
    {


        //  L’utilisateur peut revenir au menu par le même geste explicite que dans le mode libre.
        EventManager.addActionToEvent<GesteTypes>(MyEventTypes.GESTE_DETECTED, clapToQuit);
    }

    public void clapToQuit(GesteTypes type)
    {
        if(type == GesteTypes.CLAP)
            EventManager.raise<ScenesType>(MyEventTypes.CHANGE_SCENE, ScenesType.MAIN_MENU);
    }
}
