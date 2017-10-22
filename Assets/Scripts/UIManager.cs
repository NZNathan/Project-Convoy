using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public static UIManager instance;

    public VehicleUI baseVehicleUI;

    // Use this for initialization
    void Start()
    {
        #region Singleton
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }

        instance = this;
        #endregion
    }

    public VehicleUI newVehicleUI(Vehicle vehicle)
    {
        VehicleUI vehicleUI = Instantiate(baseVehicleUI, transform);

        vehicleUI.setVehicle(vehicle.transform);

        return vehicleUI;
    }
}
