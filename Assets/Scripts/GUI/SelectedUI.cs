using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedUI : MonoBehaviour {

    //Wrapper
    public GameObject wrapperObject;

    //Target Vehicle 
    private Vehicle vehicle;

    [Header("Information Components")]
    public Text nameText;
    public Text healthText;

    public void setActive(bool value, Vehicle target)
    {
        wrapperObject.SetActive(value);

        //If being activated update text
        if (value)
        {
            vehicle = target;

            //Update Stats
            nameText.text = vehicle.name;
            healthText.text = "Health: " + vehicle.getHealth();
        }
    }

    private void Update()
    {
        if (wrapperObject.activeInHierarchy)
        {
            //Update Stats TODO: optimize so only set when it changes
            nameText.text = vehicle.name;
            healthText.text = "Health: " + vehicle.getHealth();
        }
    }
}
