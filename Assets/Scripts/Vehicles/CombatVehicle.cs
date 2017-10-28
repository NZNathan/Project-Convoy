using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Combatable))]
public class CombatVehicle : Vehicle {

    [Header("Combat Variables")]
    //Components
    public GameObject turret;

    //Combat Variables
    private Combatable combatType;

    // Use this for initialization
    private new void Start ()
    {
        base.Start();

        //Get Components
        combatType = GetComponent<Combatable>();

        //Set Vehicle type
        combatable = true;
    }

    #region Getters & Setters

    //SETTERS
    public void setAttackTarget(Vehicle target)
    {
        combatType.setAttackTarget(target);
    }

    //GETTERS
    public Combatable getCombat()
    {
        return combatType;
    }
    #endregion

    // Update is called once per frame
    public override void update()
    {
        combatType.attack(this);
	}
}
