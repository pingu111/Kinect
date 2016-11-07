

using UnityEngine;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

namespace EnumJoint
{
    public enum jointType
    {
        SpineBase, SpineMid,  Head, ShoulderLeft, ElbowLeft, WristLeft,
        HandLeft, ShoulderRight, ElbowRight, WristRight, HandRight, HipLeft,
        HipRight, SpineShoulder, HandTipLeft, ThumbLeft, HandTipRight, ThumbRight
    };
}

public class Movement : MonoBehaviour
{
    List<Vector3> listPosition = new List<Vector3>();

    public EnumJoint.jointType joint;

    public GameObject BodySourceManager;

    private BodySourceManager _BodyManager;

    // Use this for initialization
    void Start()
    {
        this.gameObject.name = joint.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (BodySourceManager == null)
        {
            return;
        }

        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            return;
        }

        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }

        foreach (var body in data)
        {
            if (body.IsTracked)
            {
                Vector3 partPosition = GetVector3FromJoint(body.Joints[Kinect.JointType.SpineBase + (int)joint]);
                this.gameObject.transform.position = partPosition;
                listPosition.Add(this.gameObject.transform.position);
            }
        }
    }

    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }

    void OnDestroy()
    {
        using (System.IO.StreamWriter file =
        new System.IO.StreamWriter(@".\PositionHand.txt"))
        {
            foreach (Vector3 line in listPosition)
            {
                // If the line doesn't contain the word 'Second', write the line to the file.
                file.WriteLine(line.ToString().Replace(",","."));
            }
        }
    }
}
