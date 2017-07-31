using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEditor;


public class Controller : MonoBehaviour
{
    public float energy = 100f;
    public float enginePower = 10f;

    public float gravity = 0.02f;

    public float speed;

    private Rigidbody rb;
    public Camera cam;
    public GameObject ui;

    private Quaternion targetRotation;

    //UI
    public Scrollbar energyBar;
    public Text energyText;
    public Text speedText;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        // cam = GetComponentInChildren<Camera>();
        // cam.enabled = false;

        targetRotation = transform.rotation;
    }

    private void Update()
    {
        //Camera
        Vector3 camChaseSpot = transform.position - transform.forward * 10.0f + Vector3.up * 5.0f;
        float chaseBias = 0.9f;

        Camera.main.transform.position = chaseBias * Camera.main.transform.position + (1.0f - chaseBias) * camChaseSpot;
        Camera.main.transform.LookAt(transform.position + transform.forward * 20.0f);


        if (Input.GetKey(KeyCode.Space))
        {
            if (energy > 0 )
            {
                speed += enginePower * Time.deltaTime;
                energy -= enginePower * Time.deltaTime;
            }
        }

        //Speed -- en fonction de la hauteur
        speed -= transform.forward.y * 0.2f;

        if (speed < 5.0f)
        {
            speed = 5.0f;
        }

        float controlEffect = speed / 50.0f;

        //transform.position += transform.forward * Time.deltaTime * speed;
        transform.Rotate(controlEffect * Input.GetAxis("Vertical"), 0.0f, -controlEffect * Input.GetAxis("Horizontal"));

        //Translation
        
        //transform.position += transform.forward * Time.deltaTime * speed;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        //Gravité
        rb.AddForce(Physics.gravity * rb.mass * gravity);


        //UI
        speedText.text = "Speed : " + Mathf.Round(speed).ToString();

        var pourEnergy = (energy - 0) / (100 - 0);
        energyBar.size = pourEnergy;
        
        if (energy > 0)
            energyText.text = "Energy : " + Mathf.Round(energy).ToString();
        else
            energyText.text = "Energy tank empty";
    }

    public void Death()
    {
        GameObject go = GameObject.FindGameObjectWithTag("RestartManager");
        go.GetComponent<RestartManager>().Restart();
        Destroy(gameObject);
    }

    public void Launch()
    {
        //rb.AddForce(transform.forward * 8000f, ForceMode.Acceleration);
        speed += 100f;
    }

    public void Win()
    {
        GameObject go = GameObject.FindGameObjectWithTag("RestartManager");
        go.GetComponent<RestartManager>().Win();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Terrain")
            Death();



    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Win")
            Win();
    }

   /* void OnDrawGizmos()
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
    */

}