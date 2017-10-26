using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum VehicleType { CONVOY, COMBAT };

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Moveable))]
public class Vehicle : Pauseable {

    //Components
    protected Animator animator;
    protected Rigidbody rb;
    private Moveable movement;

    //Raycast offest
    public static Vector3 rayOffest = new Vector3(0f, 0.45f, 0f);

    //UI
    protected VehicleUI vehicleUI;

    //Movement Variables
    protected bool moving = false;

    //Vehicle type
    protected bool combatable;

    [Header("Health Variables")]
    public int maxHealth;
    protected int health;
    protected bool dead = false;

	// Use this for initialization
	public void Start ()
    {
        //Get Components
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<Moveable>();

        //Create a new UI for the vehicle
        vehicleUI = UIManager.instance.newVehicleUI(this);
        vehicleUI.setActive(true);

        //Set Vehicle type
        combatable = false;

        //Set up stats
        health = maxHealth;
    }

    public void takeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            death();
        }
    }

    protected virtual void death()
    {
        Destroy(vehicleUI.gameObject);
        Destroy(this.gameObject);
    }

    private void OnMouseEnter()
    {
        vehicleUI.setActive(true);
    }

    private void OnMouseExit()
    {
        vehicleUI.setActive(false);
    }

    #region Getters & Setters

    //GETTERS
    public Rigidbody getRigidbody()
    {
        return rb;
    }

    public bool isDead()
    {
        return dead;
    }

    public bool isCombatable()
    {
        return combatable;
    }

    public int getHealth()
    {
        return health;
    }

    public Moveable getMovement()
    {
        return movement;
    }

    public Animator getAnimator()
    {
        return animator;
    }

    //SETTERS
    public void setTargetPosition(Vector3 newTargetPos)
    {
        movement.setTargetPosition(newTargetPos);
        moving = true;
    }

    public void setMoving(bool moving)
    {
        this.moving = moving;
    }

    #endregion

    public override void fixedUpdate()
    {
        if (moving)
        {
            movement.moveToGird(this);
        }
	}
}
