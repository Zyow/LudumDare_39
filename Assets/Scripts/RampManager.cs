using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampManager : MonoBehaviour
{
    public Transform hookLauncher;
    public Transform transformRamp;

    public float angleRamp;
    public float speedRot;
    public float minRot = 0f;
    public float maxRot = -30f;

    public bool lauchTime = false;
    public GameObject playerPlane;


	void Start () {
        StartLauch();

    }

    public void StartLauch()
    {
        if (playerPlane == null)
            playerPlane = GameObject.FindGameObjectWithTag("Player");

        if (playerPlane != null)
        {
            playerPlane.transform.position = hookLauncher.transform.position;
            playerPlane.transform.rotation = hookLauncher.transform.rotation;
        }

        lauchTime = true;
    }
	
	void Update ()
    {
        if (lauchTime)
        {

            angleRamp += Input.GetAxis("Mouse Y") * -speedRot * Time.deltaTime;
            angleRamp = Mathf.Clamp(angleRamp, -30.0f, 0f);
            transform.rotation = Quaternion.Euler(angleRamp, 0.0f, 0.0f);

            playerPlane.transform.position = hookLauncher.transform.position;
            playerPlane.transform.rotation = hookLauncher.transform.rotation;

            if (Input.GetMouseButtonDown(0))
            {
                if (playerPlane.GetComponent<Controller>())
                {
                    GetComponent<Animator>().SetTrigger("LaunchTrigger");
                }
            }

        }
    }

    void LaunchTime()
    {
        playerPlane.GetComponent<Controller>().Launch();
        lauchTime = false;
    }



}
