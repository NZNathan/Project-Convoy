using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleUI : MonoBehaviour {

    private bool uiOn = false;

    public GameObject uiWrapper;

    [Header("Positioning")]
    public float xOffest = 0;
    public float yOffeset = 0;
    private Transform vehicle;

    private void Start()
    {
        xOffest = Screen.width * (xOffest / 1920); //Normalize value for screen resolution
        yOffeset = Screen.height * (yOffeset / 1080); //divide by 1920 and 1080 as thats the resolution i built the ui for

        //Turn off ui at start
        uiWrapper.SetActive(false);
    }

    public void setVehicle(Transform vehicle)
    {
        this.vehicle = vehicle;
    }

    public void setActive(bool value)
    {
        uiOn = value;
        uiWrapper.SetActive(value);
    }

    private void Update()
    {
        if (uiOn)
        {
            float yBounds = vehicle.GetComponent<Collider>().bounds.size.y;

            Vector3 vehiclePos = vehicle.position;
            Vector3 uiPos = CameraController.activeCamera.WorldToScreenPoint(new Vector3(vehiclePos.x + xOffest, vehiclePos.y + yBounds + yOffeset, vehiclePos.z)); //TODO: Set it to be above the vehicle

            transform.position = uiPos;
        }
    }
}
