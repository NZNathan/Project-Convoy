using System.Collections;
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
    /// Raycasts and sets the hit object as the selected object
    /// </summary>
    private void selectObject()
    {
        //Get the camera
        Camera mainCamera = Camera.main;

        //Ray variables
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.black, 50 );

        if (Physics.Raycast(ray, out hit, 100, selectableMask)) 
        {
            Debug.Log(hit.transform.gameObject.name + " at (" + ray.origin.x + ", 0, " + ray.origin.z + ")");

            //Check clicked object is a vehicle
            if(hit.transform.tag == "Vehicle")
                selected = hit.transform.gameObject.GetComponent<Vehicle>();
        }
        else
        {
            //if left clicking nothing unselect what was selected
            selected = null;
        }
    }

    /// <summary>
    /// Raycasts and converts the point to the closest grid position, then moves the selected object to that position
    /// </summary>
    private void selectGrid()
    {
        //If there is nothing selected, then can't do any actions with right click
        if (selected == null)
            return;

        //Get the camera
        Camera mainCamera = Camera.main;

        //Ray variables
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.black, 50);

        if (Physics.Raycast(ray, out hit, 100, groundMask))
        {
            //Get the impact point on the ray to get the position on the ground that was hit
            Vector3 ground = ray.GetPoint(hit.distance);

            //if point is off grid
            if (ground.x < 0 || ground.x > GridManager.instance.xBound() || ground.z < 0 || ground.z > GridManager.instance.zBound())
                return;

            //Round point to a grid location
            Vector3 gridPoint = new Vector3(snapToGird(ground.x), 0f, snapToGird(ground.z));

            //DEBUG
            //Debug.Log("(" + ground.x + ", " + ground.y + ", " + ground.z + ")");
            //Debug.Log("(" + gridPoint.x + ", " + gridPoint.y + ", " + gridPoint.z + ")");

            //Move selected Object
            selected.setTargetPosition(gridPoint);
        }
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
            selectGrid();
    }
	
	// Update is called once per frame
	void Update ()
    {
        getInput();
    }
}

