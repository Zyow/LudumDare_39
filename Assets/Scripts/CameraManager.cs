using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCam;
    private Transform cameraRig;
    private Vector3 lastMousePos;

    //Camera orbit options
    public float orbitSens = 10f;
    bool holdToOrbit = false;

    //Zoom options
    public float ZoomMultiplier = 2f;
    public float minDist = 2f;
    public float maxDist = 25f;
    public bool InvertZoomDir = false;

    private void Start()
    {
        mainCam = Camera.main;
        cameraRig = mainCam.transform.parent;
    }

    void Update ()
    {
        OrbitCamera();
        ZoomCamera();
    }

    void ZoomCamera()
    {
        float delta = -Input.GetAxis("Mouse ScrollWheel");

        if (InvertZoomDir)
            delta = -delta;

        Vector3 actualChange = mainCam.transform.localPosition * ZoomMultiplier * delta;
        Vector3 newPosition = mainCam.transform.localPosition + actualChange;

        newPosition = newPosition.normalized * Mathf.Clamp(newPosition.magnitude, minDist, maxDist);
        mainCam.transform.localPosition = newPosition;
    }

    void OrbitCamera()
    {
        if (Input.GetMouseButtonDown(2))
            lastMousePos = Input.mousePosition;

        if (Input.GetMouseButton(2))
        {
            Vector3 currentMousePos = Input.mousePosition;
            Vector3 mouseMov = currentMousePos - lastMousePos;
            Vector3 posRelativToRig = mainCam.transform.localPosition;
            Vector3 rotationAngles = mouseMov / orbitSens;

            if (holdToOrbit)
                rotationAngles *= Time.deltaTime;

            mainCam.transform.RotateAround(cameraRig.position, mainCam.transform.right, -rotationAngles.y);
            mainCam.transform.RotateAround(cameraRig.position, mainCam.transform.up, rotationAngles.x);

            Quaternion lookRotation = Quaternion.LookRotation(-mainCam.transform.localPosition);
            mainCam.transform.rotation = lookRotation;

            if (!holdToOrbit)
                lastMousePos = currentMousePos;
        }
    }
}
