using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}

    private void checkClick()
    {
        Camera mainCamera = Camera.main;

        // We need to actually hit an object
        RaycastHit hit = new RaycastHit();

        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 newPos = new Vector3(-mousePos.x, mousePos.y, -mousePos.z);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.black, 50 );

        if (Physics.Raycast(ray, out hit, 100)) 
        {
            Debug.Log(hit.transform.gameObject.name);
        }
        else
            Debug.Log("miss");
    }
	
	// Update is called once per frame
	void Update ()
    {
        bool leftClick = Input.GetMouseButtonDown(0);

        if (leftClick)
            checkClick();
	}
}

