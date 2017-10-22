using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum VehicleType { CONVOY, COMBAT };

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Moveable))]
public class Vehicle : Pauseable {

    //Components
    protected Rigidbody rb;

    //Movement Variables
    private Moveable movement;
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
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<Moveable>();

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
        Destroy(this.gameObject);
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
