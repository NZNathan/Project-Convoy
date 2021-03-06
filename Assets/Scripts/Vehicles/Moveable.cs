﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour {

    //Movement Variables
    public float movespeed = 0.1f;

    //Positioning Variables
    private Vector3[] targetPositions;
    private int pathIndex = 0;
    private float snapRange = 0.05f;
    private float turnAngle = 20f;

    /// <summary>
    /// Moves the vehicle toward the targetPos by the vehicles movement speed
    /// </summary>
    /// <param name="vehicle"></param>
    /// <param name="targetPos"></param>
	public void moveToGird(Vehicle vehicle)
    {
        //If no path to follow
        if(targetPositions.Length == 0)
        {
            vehicle.setMoving(false);
            return;
        }

        //Get current point in the path
        Vector3 targetPos = targetPositions[pathIndex];

        //Angle vehicle to turn
        setAngle(vehicle);

        //Get direction and normalize it so the speed is constant over the whole movement
        Vector3 dir =  (targetPos - vehicle.transform.position).normalized;

        //Move vehicle
        vehicle.getRigidbody().MovePosition(vehicle.transform.position + (dir * Time.deltaTime * movespeed));

        //Check to see if reached targetPosition
        if ((targetPos - vehicle.transform.position).magnitude < snapRange)
        {
            
            pathIndex++;
            if (pathIndex == targetPositions.Length)
            {
                //stop moving if reached goal
                vehicle.setMoving(false);
                //Snap to the goal square
                snapPosition(vehicle, targetPos);
                //Update grid square to be impassable
                GridManager.instance.setWalkable(targetPos, false);
            }
        }
    }

    /// <summary>
    /// Sets the angle of the vehicle to match the way they are moving
    /// </summary>
    /// <param name="vehicle"></param>
    /// <param name="targetPos"></param>
    public void setAngle(Vehicle vehicle)
    {
        //Get current point in the path
        Vector3 targetPos = targetPositions[pathIndex];

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
        else
        {
            vehicle.transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }

    /// <summary>
    /// Moves the vehicle one square in any direction (1 = Left, 2 = Right 3 = Forward, 4 = Back) returns true on a successful move
    /// </summary>
    /// <param name="vehicle"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    public bool movePosition(Vehicle vehicle, int direction, bool check = true)
    {
        targetPositions = new Vector3[1];
        Vector3 movePosition = GridManager.instance.snapToGrid(transform.position);

        //Move left
        if (direction == 1)
            movePosition = new Vector3(movePosition.x - GridManager.instance.tileSize, movePosition.y, movePosition.z);
        //Move right
        else if (direction == 2)
            movePosition = new Vector3(movePosition.x + GridManager.instance.tileSize, movePosition.y, movePosition.z);
        //Move forward
        else if (direction == 3)
            movePosition = new Vector3(movePosition.x, movePosition.y, movePosition.z + GridManager.instance.tileSize);
        //Move backward
        else if (direction == 4)
            movePosition = new Vector3(movePosition.x, movePosition.y, movePosition.z - GridManager.instance.tileSize);

        //Only move if space is empty
        if (Selector.gridSpaceEmpty(movePosition) || !check)
        {
            vehicle.setMoving(true);
            GridManager.instance.setWalkable(transform.position, true);
            targetPositions[0] = movePosition;
            pathIndex = 0;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Snap the vehicle to the target position and set moving to false for the vehicle
    /// </summary>
    /// <param name="vehicle"></param>
    /// <param name="targetPos"></param>
    public void snapPosition(Vehicle vehicle, Vector3 snapPos)
    {
        //Stop movement
        vehicle.setMoving(false);

        //Snap to targetPos
        vehicle.getRigidbody().position = snapPos;

        //Reset rotation
        vehicle.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    public void setTargetPosition(Vector3 newPosition)
    {
        Vector3 startPoint = GridManager.instance.snapToGrid(transform.position);
        Vector3 endPoint = GridManager.instance.snapToGrid(newPosition);

        Vector3[] path = Pathfinding.FindPath(startPoint, endPoint);

        if (path != null)
        {
            GridManager.instance.setWalkable(startPoint, true);
            targetPositions = path;
            pathIndex = 0;
        }
        else
        {
            targetPositions = new Vector3[0];
        }
    }
}
