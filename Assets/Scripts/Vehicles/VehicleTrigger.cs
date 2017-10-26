using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleTrigger : MonoBehaviour {

    Animator animator;
    Combatable combat;

    private void Start()
    {
        animator = GetComponent<Animator>();
        combat = GetComponentInParent<Combatable>();
    }

    public void resetRamInt()
    {
        animator.SetInteger("ram", 0);
    }

    public void setAttackTrigger()
    {
        combat.attackTrigger = true;
    }
}
