using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour {

    public static UIManager instance;

    //UI Components
    public VehicleUI baseVehicleUI;
    public SelectedUI selectedUI;

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

        vehicleUI.setVehicle(vehicle);

        return vehicleUI;
    }

    public void toggleSelectedUI(bool value, Vehicle target)
    {
        selectedUI.setActive(value, target);
    }

    public static bool cursorOnUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
