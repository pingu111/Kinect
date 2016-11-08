using UnityEngine.UI;
using System.Collections;
using UnityEngine;

public class Informations
{
    public string nom;
    public string age;
    public string taille;
    public string frequence;
    public bool isRightHanded;
}


public class GetInformationsScript : MonoBehaviour
{

    public Toggle isRightHanded;
    public Text nom;
    public Text age;
    public Text taille;
    public Text frequence;

    public Informations userInfos;

    // Use this for initialization
    void Start ()
    {
        if (FindObjectsOfType<GetInformationsScript>().Length > 1)
            Destroy(this.gameObject);
        else
        {
            DontDestroyOnLoad(this.gameObject);
            EventManager.addActionToEvent(MyEventTypes.GET_INFORMATIONS, getInformationsThenLeave);
        }

    }

    public void getInformationsThenLeave()
    {
        userInfos = new Informations();
        userInfos.nom = nom.text;
        userInfos.age = age.text;
        userInfos.taille = taille.text;
        userInfos.frequence = frequence.text;
        userInfos.isRightHanded = isRightHanded.isOn;

        EventManager.raise<ScenesType>(MyEventTypes.CHANGE_SCENE, ScenesType.DETECTION);
    }


}
