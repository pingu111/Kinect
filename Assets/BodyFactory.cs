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

            switch(part.GetComponent<Movement>().joint)
            {
                case jointType.HandLeft:
                    this.gameObject.GetComponent<BodyState>().leftHand = part;
                    break;
                case jointType.HandRight:
                    this.gameObject.GetComponent<BodyState>().rightHand = part;
                    break;
                case jointType.ShoulderLeft:
                    this.gameObject.GetComponent<BodyState>().leftShoulder = part;
                    break;
                case jointType.ShoulderRight:
                    this.gameObject.GetComponent<BodyState>().rightShoulder = part;
                    break;
                case jointType.SpineMid:
                    this.gameObject.GetComponent<BodyState>().middleBody = part;
                    break;
                default:
                    break;
            }
        }
	}
}
