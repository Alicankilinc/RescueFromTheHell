using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public HingeJoint hj;
    JointMotor motor;
    float axis;

    // Start is called before the first frame update
    void Start()
    {
        motor = hj.motor;   
    }

    // Update is called once per frame
    void Update()
    {
        axis = hj.angle;
        motor.targetVelocity = -axis;
        hj.motor = motor;
    }
}
