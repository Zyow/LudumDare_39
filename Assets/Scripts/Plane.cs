using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Plane : MonoBehaviour {

    public bool canFly = false;

    public float speed = 10f;

    public Rigidbody rb;
    public Transform newbcenterOfMass;

    public float gravity = 1f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.centerOfMass = newbcenterOfMass.localPosition;
    }

    void Update()
    {
        if(Input.GetKeyDown("e"))
        {
            rb.AddForce(transform.forward * 8000f, ForceMode.Acceleration);
        }

        if (Input.GetKeyDown("r"))
        {
            rb.constraints = RigidbodyConstraints.None;
        }

    }

}
