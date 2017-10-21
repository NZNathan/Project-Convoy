using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {

    //Components
    private Rigidbody rb;

    //Movement Variables
    public float moveSpeed = 0.1f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void setup(Transform target)
    {
        rb = GetComponent<Rigidbody>();

        //Set Rotation
        //Only want the z and x position of target, and y of turret so target doesn't aim up or down
        Vector3 targetPosition = new Vector3(target.position.x, transform.position.y, target.position.z);

        transform.LookAt(targetPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Vehicle")
            Destroy(this.gameObject);
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        rb.MovePosition(transform.position + transform.forward * Time.deltaTime * moveSpeed);
	}
}
