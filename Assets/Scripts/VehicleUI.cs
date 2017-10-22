using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleUI : MonoBehaviour {

    private bool uiOn = false;

    [Header("Positioning")]
    public float xOffest = 0;
    public float yOffeset = 0;
    private Transform vehicle;

    private void Start()
    {
        xOffest = Screen.width * (xOffest / 1920); //Normalize value for screen resolution
        yOffeset = Screen.height * (yOffeset / 1080); //divide by 1920 and 1080 as thats the resolution i built the ui for
    }

    public void setVehicle(Transform vehicle)
    {
        this.vehicle = vehicle;
    }

    public void setActive(bool value)
    {
        uiOn = value;
    }

    private void Update()
    {
        if (uiOn)
        {
            Vector3 vehiclePos = CameraController.activeCamera.WorldToScreenPoint(vehicle.position); //TODO: Set it to be above the vehicle

            transform.position = new Vector3(vehiclePos.x + xOffest, vehiclePos.y + yOffeset, vehiclePos.z);
        }
    }
}
