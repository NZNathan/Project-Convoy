using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An object that will not act when time is not moving
/// </summary>
public abstract class Pauseable : MonoBehaviour {


    public virtual void update() { }
    public virtual void fixedUpdate() { }
    public virtual void lateUpdate() { }

    void Update ()
    {
        //Only do actions if time is not stopped
        if (!TimeManager.instance.isTimeStopped())
        {
            update();
        }
	}

    void LateUpdate()
    {
        //Only do actions if time is not stopped
        if (!TimeManager.instance.isTimeStopped())
        {
            lateUpdate();
        }
    }

    void FixedUpdate()
    {
        //Only do actions if time is not stopped
        if (!TimeManager.instance.isTimeStopped())
        {
            fixedUpdate();
        }
    }
}
