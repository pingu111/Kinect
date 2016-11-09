using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class RandomGest : MonoBehaviour {

    private int numberLoop = 0;
    private int nbLoopsToDo = 2;

    public List<Pair< GesteTypes, GesteTypes>> listGestsToDoAndDetected = new List<Pair<GesteTypes, GesteTypes>>();

    /// <summary>
    /// listGesteDetected[nbloop][idgeste]
    /// </summary>

    private float timeToDetect = 10.0f;
    private float timeActual = 0;

    private float timeToBegin = 3.0f;
    private float timeActualBegin = 0;

    private int actualIndex = 0;

    public Dictionary<GesteTypes, int> dicoNbSuccededGest = new Dictionary<GesteTypes, int>();

    public GameObject sprite;

	// Use this for initialization
	void Start ()
    {
        createListRandomGeste();
        EventManager.addActionToEvent<GesteTypes>(MyEventTypes.GESTE_DETECTED, gesteDetected);
        askForGest();
    }

    void Update()
    {
        timeActualBegin += Time.deltaTime;
        if(timeActualBegin > timeToBegin)
        {
            timeActual += Time.deltaTime;
            if (timeActual > timeToDetect && numberLoop < nbLoopsToDo)
            {
                listGestsToDoAndDetected[actualIndex].Second = (GesteTypes.NO_GESTES_TIMER);
                askForNewGeste();
                actualIndex++;
            }
            else
                sprite.transform.localScale = new Vector3(Mathf.Max((1 - timeActual / timeToDetect), 0), sprite.transform.localScale.y, 1);
        }
        else
        {
            this.gameObject.GetComponent<Text>().text = "Preparez vous !";
        }
    }

    void askForNewGeste()
    {
        timeActual = 0;
        if (actualIndex == listGestsToDoAndDetected.Count)
        {
            if (numberLoop >= nbLoopsToDo)
            {
                EventManager.removeActionFromEvent<GesteTypes>(MyEventTypes.GESTE_DETECTED, gesteDetected);
                stockerInfos();
            }
        }
        askForGest();
    }

    void gesteDetected(GesteTypes _type)
    {
        if (timeActualBegin > timeToBegin)
        {
            listGestsToDoAndDetected[actualIndex].Second = _type;
            if (_type == listGestsToDoAndDetected[actualIndex].First)
            {
                askForNewGeste();
                actualIndex++;

                dicoNbSuccededGest[_type]++;
            }
            else if (timeActual > 1.0f)
            {
                Debug.Log("Mauvais geste " + _type + " au lieu de " + listGestsToDoAndDetected[actualIndex].First);
                askForNewGeste();
                actualIndex++;
            }
        }
    }

    void askForGest()
    {
        GesteTypes newGeste = listGestsToDoAndDetected[actualIndex].First;
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
                newText = "Main droite vers la gauche ";
                break;
            case GesteTypes.SWIPE_RIGHT_WITH_RIGHT_HAND:
                newText = "Main droite vers la droite ";
                break;
            default:
                break;
        }

        this.gameObject.GetComponent<Text>().text = newText;
    }

    private void createListRandomGeste()
    {

        for (int i = 0; i < nbLoopsToDo; i ++)
        {
            List<GesteTypes> listGestsToDo = new List<GesteTypes>();
            Array values = Enum.GetValues(typeof(GesteTypes));

            // remove of the clap
            while (values.Length - 2 != listGestsToDo.Count)
            {
                System.Random random = new System.Random();
                GesteTypes randomValue = (GesteTypes)values.GetValue(random.Next(values.Length));
                if (!listGestsToDo.Contains(randomValue) && randomValue != GesteTypes.CLAP && randomValue != GesteTypes.NO_GESTES_TIMER)
                {
                    listGestsToDo.Add(randomValue);
                }
            }

            foreach (GesteTypes g in listGestsToDo)
                listGestsToDoAndDetected.Add(new Pair<GesteTypes, GesteTypes>(g, GesteTypes.NO_GESTES_TIMER));
        }

        foreach(GesteTypes values in Enum.GetValues(typeof(GesteTypes)))
        {
            if (values != GesteTypes.CLAP && values != GesteTypes.NO_GESTES_TIMER)
                dicoNbSuccededGest.Add(values, 0);
        }
    }

    public void stockerInfos()
    {
        float globalPercent = 0;

        Dictionary<GesteTypes, float> dicoPercentage = new Dictionary<GesteTypes, float>();
        foreach(KeyValuePair<GesteTypes, int> p in dicoNbSuccededGest)
        {
            float percent = 100 * p.Value / nbLoopsToDo;

            globalPercent += percent / dicoNbSuccededGest.Count;
            dicoPercentage.Add(p.Key, percent);
        }

        string success = "";
        success += "Global % : " + globalPercent + "\n";
        foreach (KeyValuePair<GesteTypes, float> p in dicoPercentage)
        {
            success += "Geste : " + p.Key + " / % : " + p.Value + "\n";
        }

        System.Random r = new System.Random();
        int randomNumber = r.Next(1, 999999999);
        string id = "ID : " + randomNumber + "\n";

        Informations userInfo = GameObject.FindObjectOfType<SceneManager>().userInfos;

        string user = "Nom : " + userInfo.nom + "\n";
        user += "Age : " + userInfo.age + "\n";
        user += "Taille : " + userInfo.taille + "\n";
        user += "Main principale droite : " + userInfo.isRightHanded + "\n";
        user += "Frequence : " + userInfo.frequence + "\n";

        string gestsDetected ="";
        foreach(Pair<GesteTypes, GesteTypes> p in  listGestsToDoAndDetected)
        {
            gestsDetected += p.First + " pour " + p.Second + "\n";
        }

        string total = id + user + success + gestsDetected;
        string name = (userInfo.nom + randomNumber) +".txt";
        Debug.Log(total);
        System.IO.File.WriteAllText(name, total);


        //  L’utilisateur peut revenir au menu par le même geste explicite que dans le mode libre.
        EventManager.addActionToEvent<GesteTypes>(MyEventTypes.GESTE_DETECTED, clapToQuit);
    }

    public void clapToQuit(GesteTypes type)
    {
        if(type == GesteTypes.CLAP)
            EventManager.raise<ScenesType>(MyEventTypes.CHANGE_SCENE, ScenesType.MAIN_MENU);
    }

    void OnDestroy()
    {
        EventManager.removeActionFromEvent<GesteTypes>(MyEventTypes.GESTE_DETECTED, clapToQuit);

    }
}
