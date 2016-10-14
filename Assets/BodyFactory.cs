using UnityEngine;
using System.Collections;
using System;
using EnumJoint;

public class BodyFactory : MonoBehaviour
{
    public GameObject prefabBodyPart;

    public GameObject bodyManager;

    // Use this for initialization
    void Start ()
    {
        for(int i = 0; i < Enum.GetNames(typeof(jointType)).Length; i ++)
        {
            GameObject part =  Instantiate(prefabBodyPart);
            part.GetComponent<Movement>().joint = (jointType)i;
            part.GetComponent<Movement>().BodySourceManager = bodyManager;
            part.transform.parent = this.transform;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
