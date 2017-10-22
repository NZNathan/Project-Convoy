using System.Collections;
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

            //If able to attack because of attack delay
            if(lastAttack + attackDelay < TimeManager.instance.getTime())
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

    private void aimAtTarget(CombatVehicle vehicle)
    {
        //Only want the z and x position of target, and y of turret so target doesn't aim up or down
        Vector3 targetPosition = new Vector3(attackTarget.transform.position.x, vehicle.turret.transform.position.y, attackTarget.transform.position.z);

        vehicle.turret.transform.LookAt(targetPosition);
    }

    #region Getters & Setters

    //SETTERS
    public void setAttackTarget(Vehicle target)
    {
        attackTarget = target;
    }

    //GETTERS
    public Vehicle getAttackTarget()
    {
        return attackTarget;
    }

    #endregion
}
