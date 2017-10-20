using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MoveVehicle))]
public class Vehicle : MonoBehaviour {

    //Components
    private Rigidbody rb;

    //Movement Variables
    private MoveVehicle movement;
    private Vector3 targetPos;
    private bool moving = false;

    //Attack Variables

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<MoveVehicle>();
    }

    #region Getters & Setters

    //GETTERS
    public Rigidbody getRigidbody()
    {
        return rb;
    }

    //SETTERS
    public void setTargetPosition(Vector3 newTargetPos)
    {
        targetPos = newTargetPos;
        moving = true;
    }

    public void setMoving(bool moving)
    {
        this.moving = moving;
    }

    #endregion

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (moving)
        {
            movement.moveToGird(this, targetPos);
        }
	}
}
