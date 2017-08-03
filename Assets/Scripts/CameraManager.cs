using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    //Var Camera
    [SerializeField]
    private Camera mainCam;

    //Cam Stat
    public enum State
    {
        Menu,
        Editor,
        Flying,
    }
    public State camState;

    //Position Camera
    public Transform startPoint;
    [SerializeField]
    private Vector3 camPivot;
    private Vector3 lastMousePos;

    //Camera orbit options
    public float orbitSens = 10f;
    bool holdToOrbit = false;

    //Zoom options
    public float ZoomMultiplier = 2f;
    public float minDist = 2f;
    public float maxDist = 25f;
    public bool InvertZoomDir = false;

    //A Supprimer
    public GameObject testPivot;

    void Start()
    {
        mainCam = Camera.main;
        StateMenu();
    }

    void Update ()
    {
        CamSwitch();
    }

    public void NewRoot(Vector3 t)
    {
        camPivot = t;
    }

    public void StateMenu()
    {
        camState = State.Menu;
    }

    public void StateEditor()
    {
        camState = State.Editor;
    }

    public void StateFlying()
    {
        camState = State.Flying;
    }


    void CamSwitch()
    {
        switch (camState)
        {
            //Camera MenuPrincipal
            case State.Menu:

                MenuCamera();

                break;

            //Camera Editeur
            case State.Editor:

                OrbitCamera();
                //ZoomCamera();

                break;
        }
    }

    void MenuCamera()
    {
        mainCam.transform.position = startPoint.position;
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

            testPivot.transform.position = camPivot;

            if (holdToOrbit)
                rotationAngles *= Time.deltaTime;

            mainCam.transform.RotateAround(camPivot, mainCam.transform.right, -rotationAngles.y);
            mainCam.transform.RotateAround(camPivot, mainCam.transform.up, rotationAngles.x);

            Quaternion lookRotation = Quaternion.LookRotation(-mainCam.transform.localPosition);
            mainCam.transform.rotation = lookRotation;

            if (!holdToOrbit)
                lastMousePos = currentMousePos;
        }
    }
}
