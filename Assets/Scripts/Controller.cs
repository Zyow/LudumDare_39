using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Controller : MonoBehaviour
{

    public float enginePower = 10f;

    public float gravity = 10f;

    public float speed;

    private float mouseX;
    private float mouseY;

    public Vector3 lift;
    public float liftBooster;

    private Rigidbody rb;
    public Camera cam;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
       // cam = GetComponentInChildren<Camera>();
       // cam.enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {

        // mouseX += Input.GetAxis("Mouse X");
        // mouseY += Input.GetAxis("Mouse Y");

        // transform.Rotate(0f, mouseX * Time.deltaTime, 0f, Space.World);
        //transform.rotation = Quaternion.Euler(mouseY, transform.rotation.eulerAngles.y, -mouseX);
        //transform.Rotate(mouseY * 0.01f, 0f, 0f);

        /* if (Input.GetMouseButton(0))
         {
             // transform.Translate(0f, 0f, speed * Time.deltaTime);
             //rb.AddForce(transform.forward * enginePower * Time.deltaTime, ForceMode.Acceleration);
             rb.AddRelativeForce(transform.forward * enginePower * Time.deltaTime, ForceMode.Acceleration);
         }


         //if (enginePower > 35f)
         //   enginePower = 35f;

         if (enginePower < 0)
             enginePower = 0;

         transform.Rotate(Input.GetAxis("Mouse Y"), 0, -Input.GetAxis("Mouse X"));

         GetComponent<Rigidbody>().AddForce(-Vector3.up * gravity, ForceMode.Acceleration);

         var localVelocity = transform.InverseTransformDirection(rb.velocity);
         speed = Mathf.Max(0, localVelocity.z);*/

        //++

        Vector3 camChaseSpot = transform.position -
                        transform.forward * 10.0f +
        Vector3.up * 5.0f;
        float chaseBias = 0.9f;
        Camera.main.transform.position = chaseBias * Camera.main.transform.position +
            (1.0f - chaseBias) * camChaseSpot;
        Camera.main.transform.LookAt(transform.position + transform.forward * 20.0f);

        if (Input.GetMouseButtonDown(0))
          {
              rb.AddForce(transform.forward * enginePower);
          }

        //Add lift force ,  set liftBooster to 100 
          lift = Vector3.Project(rb.velocity, transform.forward);
          rb.AddForce(transform.up * lift.magnitude * liftBooster);

        //Banking controls, turning turning left and right on Z axis
        //rb.AddTorque(Input.GetAxis("Mouse X") * transform.forward * -1f);
        rb.AddTorque(Input.GetAxis("Horizontal") * transform.forward * -1f);

        //Pitch controls, turning the nose up and down
        //rb.AddTorque(-Input.GetAxis("Mouse Y") * transform.right);
        rb.AddTorque(Input.GetAxis("Vertical") * transform.right);

        //Set drag and angular drag according relative to speed
        /*rb.drag = 0.001f * rb.velocity.magnitude;
          rb.angularDrag = 0.01f * rb.velocity.magnitude;*/

        speed -= Time.deltaTime * 0.2f;

          var locVel = transform.InverseTransformDirection(rb.velocity);
          locVel.z = speed;
          rb.velocity = transform.TransformDirection(locVel);

        // rb.AddForce(-Vector3.up * gravity, ForceMode.Acceleration);



    }

    public void Launch()
    {
        //rb.AddForce(transform.forward * 8000f, ForceMode.Acceleration);
        speed += 50f;
    }

    void OnDrawGizmos()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.green;
        style.fontStyle = FontStyle.Bold;
        style.alignment = TextAnchor.MiddleCenter;

        Vector3 posSpeedLabel = transform.position + Vector3.up * 2;
        Handles.Label(posSpeedLabel, "Vitesse > " + speed.ToString(), style);

        DrawHelperAtCenter(- Vector3.up, Color.red, 5f);
        DrawHelperAtCenter(transform.forward , Color.green, speed);
    }

    private void DrawHelperAtCenter(
                Vector3 direction, 
                Color color, 
                float scale)
    {
        Gizmos.color = color;
        Vector3 destination = transform.position + direction * scale;
        Gizmos.DrawLine(transform.position, destination);
    }


}