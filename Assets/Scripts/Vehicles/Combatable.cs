﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combatable : MonoBehaviour {

    private Vehicle attackTarget;
    public float attackDelay = 3f;
    private float lastAttack = -55f;

    private float turretHeight = 0.8f;

    public Bullet bulletType;

	// Use this for initialization
	void Start () {
		
	}

    public void attack(CombatVehicle vehicle)
    {
        if (attackTarget != null && !attackTarget.isDead()) 
        {
            aimAtTarget(vehicle);

            if (checkLineOfSight(attackTarget))//Check line of sight before every shot?
            {
                //If able to attack because of attack delay
                if (lastAttack + attackDelay < TimeManager.instance.getTime())
                {
                    //Reset last attack time
                    lastAttack = TimeManager.instance.getTime();

                    //Create and setup bullet
                    Vector3 spawnPos = new Vector3(transform.position.x, turretHeight, transform.position.z);
                    Bullet bullet = Instantiate(bulletType, spawnPos + (vehicle.turret.transform.forward), Quaternion.identity);
                    bullet.setup(attackTarget.transform);
                }
            }
        }
    }

    private void aimAtTarget(CombatVehicle vehicle)
    {
        //Only want the z and x position of target, and y of turret so target doesn't aim up or down
        Vector3 targetPosition = new Vector3(attackTarget.transform.position.x, vehicle.turret.transform.position.y, attackTarget.transform.position.z);

        vehicle.turret.transform.LookAt(targetPosition);
    }

    /// <summary>
    /// Returns true if have clean line of sight to the target
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private bool checkLineOfSight(Vehicle target)
    {
        //Ray variables
        RaycastHit hit;

        Debug.DrawLine(transform.position + Vehicle.rayOffest, target.transform.position + Vehicle.rayOffest, Color.blue, 50);

        //Return true if the raycast hits no objects or only hits the target
        return (!Physics.Linecast(transform.position + Vehicle.rayOffest, target.transform.position + Vehicle.rayOffest, out hit) || hit.collider.gameObject == target.gameObject);
    }

    #region Getters & Setters

    //SETTERS
    /// <summary>
    /// Sets the attack target to the target, if there is line of sight
    /// </summary>
    /// <param name="target"></param>
    public void setAttackTarget(Vehicle target)
    {
        if (checkLineOfSight(target))
        {
            attackTarget = target;
        }
    }

    //GETTERS
    public Vehicle getAttackTarget()
    {
        return attackTarget;
    }

    #endregion
}
