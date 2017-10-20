using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVehicle : MonoBehaviour {

    //Movement Variables
    public float movespeed = 0.1f;

    //Positioning Variables
    private float snapRange = 0.05f;
    private float turnAngle = 20f;

    /// <summary>
    /// Moves the vehicle toward the targetPos by the vehicles movement speed
    /// </summary>
    /// <param name="vehicle"></param>
    /// <param name="targetPos"></param>
	public void moveToGird(Vehicle vehicle, Vector3 targetPos)
    {
        //Angle vehicle to turn
        setAngle(vehicle, targetPos);

        //Get direction and normalize it so the speed is constant over the whole movement
        Vector3 dir =  (targetPos - vehicle.transform.position).normalized;

        //Move vehicle
        vehicle.getRigidbody().MovePosition(vehicle.transform.position + (dir * movespeed));

        //Check to see if reached targetPosition
        if ((targetPos - vehicle.transform.position).magnitude < snapRange)
        {
            snapPosition(vehicle, targetPos);
        }
    }

    /// <summary>
    /// Sets the angle of the vehicle to match the way they are moving
    /// </summary>
    /// <param name="vehicle"></param>
    /// <param name="targetPos"></param>
    public void setAngle(Vehicle vehicle, Vector3 targetPos)
    {
        //Turning right
        if (targetPos.x > vehicle.transform.position.x + 0.1f)
        { 
            if (targetPos.z < vehicle.transform.position.z - 0.1f)
            { //Going Backward 
                vehicle.transform.eulerAngles = new Vector3(0, -turnAngle, 0);
            }
            else //Going Forward
            {
                vehicle.transform.eulerAngles = new Vector3(0, turnAngle, 0);
            }
        }

        //Turning left
        else if (targetPos.x < vehicle.transform.position.x - 0.1f)
        {
            if (targetPos.z < vehicle.transform.position.z - 0.1f)
            { //Going Backward 
                vehicle.transform.eulerAngles = new Vector3(0, turnAngle, 0);
            }
            else //Going Forward
            {
                vehicle.transform.eulerAngles = new Vector3(0, -turnAngle, 0);
            }
        }
    }

    /// <summary>
    /// Snap the vehicle to the target position and set moving to false for the vehicle
    /// </summary>
    /// <param name="vehicle"></param>
    /// <param name="targetPos"></param>
    public void snapPosition(Vehicle vehicle, Vector3 targetPos)
    {
        //Stop movement
        vehicle.setMoving(false);

        //Snap to targetPos
        vehicle.getRigidbody().position = targetPos;

        //Reset rotation
        vehicle.transform.eulerAngles = new Vector3(0, 0, 0);
    }
}
