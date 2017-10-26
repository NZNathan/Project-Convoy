using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleTrigger : MonoBehaviour {

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void resetRamInt()
    {
        animator.SetInteger("ram", 0);
    }
}
