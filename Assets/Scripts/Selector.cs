﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour {

    //Current object that is selected
    private Vehicle selected;

    private int selectableMask;
    //Mask to use to raycast to the ground
    private int groundMask;

    // Use this for initialization
    void Start()
    {
        //So raycast only takes into account the floor quad
        groundMask = LayerMask.GetMask("Ground");

        //Mask for raycast on mouse click
        selectableMask = LayerMask.GetMask("Selectable");
    }

    /// <summary>
    /// Sets the selected vehicle to the vehicle clicked on
    /// </summary>
    private void selectObject()
    {
        Vehicle newSelect = raycastForObject();
        
        //Only update selected if the new selected is not null
        if (newSelect != null)
        {
            selected = newSelect;
        }
    }

    /// <summary>
    /// Raycasts to the point clicked and returns the vehicle at that point
    /// </summary>
    /// <returns></returns>
    private Vehicle raycastForObject()
    {
        //Get the camera
        Camera mainCamera = CameraController.activeCamera;

        //Ray variables
        RaycastHit hit = new RaycastHit();
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 20, Color.black, 50);

        if (Physics.Raycast(ray, out hit, 100, selectableMask))
        {
            Debug.Log(hit.transform.gameObject.name + " at (" + ray.origin.x + ", 0, " + ray.origin.z + ")");

            //Check clicked object is a vehicle
            if (hit.transform.tag == "Vehicle")
                return hit.transform.gameObject.GetComponent<Vehicle>();
        }
        return null;
    }

    /// <summary>
    /// Raycasts and converts the point clicked to the closest grid position and returns that position
    /// </summary>
    private Vector3 raycastForGrid()
    {
        //Get the camera
        Camera mainCamera = CameraController.activeCamera;

        //Ray variables
        RaycastHit hit = new RaycastHit();
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.black, 50);

        if (Physics.Raycast(ray, out hit, 100, groundMask))
        {
            //Get the impact point on the ray to get the position on the ground that was hit
            Vector3 ground = ray.GetPoint(hit.distance);

            //if point is off the grid then return
            if (ground.x < 0 || ground.x > GridManager.instance.xBound() || ground.z < 0 || ground.z > GridManager.instance.zBound())
                return new Vector3(-1, -1, -1); ;

            //Round point to a grid location
            Vector3 gridPoint = new Vector3(snapToGird(ground.x), 0f, snapToGird(ground.z));

            return gridPoint;
            //DEBUG
            //Debug.Log("(" + ground.x + ", " + ground.y + ", " + ground.z + ")");
            //Debug.Log("(" + gridPoint.x + ", " + gridPoint.y + ", " + gridPoint.z + ")");
        }

        return new Vector3(-1,-1,-1);
    }

    private void takeAction()
    {
        //If there is no current selected vehicle then can't take an action
        if (selected != null && !selected.isDead())
        {
            Vehicle vehicleClicked = raycastForObject();

            //See if player right clicked a Vehicle
            if (vehicleClicked == null)
            {
                //If didn't click a vehicle, instead move the current selected vehicle
                //Get point on the grid the player clicked
                Vector3 gridPoint = raycastForGrid();

                //Make sure point is on the grid before moving
                if (gridPoint != new Vector3(-1, -1, -1))
                {
                    //Move selected Object
                    selected.setTargetPosition(gridPoint);
                }
            }
            //If did click a vehicle, then attack the vehicle clicked on
            else
            {
                //Only attack if vehicle is an enemy
                if (vehicleClicked != selected)
                {
                    ((CombatVehicle)selected).setAttackTarget(vehicleClicked);
                }
            }
        }
    }

    public bool gridSpaceEmpty(Vector3 gridPosition)
    {
        Debug.DrawRay(gridPosition, Vector3.up * 5, Color.red, 50);

        return !Physics.Raycast(gridPosition, Vector3.up, 5f, selectableMask);
    }

    /// <summary>
    /// Returns the vehicle at the given grid coordinates, or null if empty
    /// </summary>
    /// <param name="gridPosition"></param>
    /// <returns></returns>
    public Vehicle getVehicleAt(Vector3 gridPosition)
    {
        RaycastHit hit = new RaycastHit();

        Debug.DrawRay(gridPosition, Vector3.up * 5, Color.red, 50);

        if(Physics.Raycast(gridPosition, Vector3.up, out hit, 5f, selectableMask))
        {
            return hit.transform.gameObject.GetComponent<Vehicle>();
        }

        return null;
    }

    /// <summary>
    /// Takes a point and rounds it to the nearest odd number
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    private float snapToGird(float point)
    {
        point = point - (point % GridManager.instance.tileSize);

        if (Mathf.Floor(point) % GridManager.instance.tileSize == 1)
            return Mathf.Floor(point);

        //If point alreadyon the grid return it as a int
        return Mathf.Ceil(point) +1;
    }

    private void getInput()
    {
        bool leftClick = Input.GetMouseButtonDown(0);
        bool rightClick = Input.GetMouseButtonDown(1);

        if (leftClick)
            selectObject();
        if (rightClick)
            takeAction();
    }
	
	// Update is called once per frame
	void Update ()
    {
        getInput();
    }
}

