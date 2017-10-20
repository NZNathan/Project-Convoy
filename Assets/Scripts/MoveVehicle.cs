using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVehicle : MonoBehaviour {

	public void moveToGird(Transform vehicle, Vector3 targetPos)
    {
        Vector3 gridPos = new Vector3(Mathf.Round(targetPos.x), 0f, Mathf.Round(targetPos.z));

        vehicle.position = gridPos;
    }
}
