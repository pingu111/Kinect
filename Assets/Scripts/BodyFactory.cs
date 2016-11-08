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
                    this.gameObject.GetComponent<BodyState>().otherHand = part;
                    break;
                case jointType.HandRight:
                    this.gameObject.GetComponent<BodyState>().masterHand = part;
                    break;
                case jointType.ShoulderLeft:
                    this.gameObject.GetComponent<BodyState>().otherShoulder = part;
                    break;
                case jointType.ShoulderRight:
                    this.gameObject.GetComponent<BodyState>().masterShoulder = part;
                    break;
                case jointType.SpineMid:
                    this.gameObject.GetComponent<BodyState>().middleBody = part;
                    break;
                case jointType.WristRight:
                    this.gameObject.GetComponent<BodyState>().rightWrist = part;
                    break;
                case jointType.HandTipRight:
                    this.gameObject.GetComponent<BodyState>().rightTip = part;
                    break;
                default:
                    break;
            }
        }
	}
}
