using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneControler : MonoBehaviour
{
    public float energyMax = 100f;              //Energie maximum disponible pour les moteurs
    public float generatedLift = 0.002f;        //Le montant de lift généré quand l'avion avance
    public float zeroLift = 300;                //La vitesse a atteindre pour désactivé l'effet lift
    public float maxEnginePower = 50f;          //Puissance maximum des moteurs
    public float throttleSpeed = 0.3f;          //Acceleration des moteurs
    public float autoRollLevel = 0.2f;
    public float autoPitchLevel = 0.2f;
    public float autoTurnPitch = 0.5f;
    public float pitchEffect = 1f;
    public float yawEffect = 0.2f;
    public float rollEffect = 1f;
    public float bankedTurnEffect = 0.5f;

    public float speed = 0f;                    //Vitesse actuelle
    public float enginePower = 0f;              //Puissance actuelle des moteurs 
    public float energy = 0f;                   //Energie actuelle
    public float throttle = 0f;                 //Regime actuelle des moteurs
    public float aeroFactor;                    //Facteur d'aerodynamisme
    public float bankedTurnAmount;

    public bool motorExist = false;             //Moteur existants?
    public bool immobilized = false;            //Avion immobilisé?

    public bool inputThrottle;                 //Input des moteurs
    public float throttlePower;                 //Puissance en fonction de l'input
    public float inputPitch;
    public float pitchAngle;
    public float inputRoll;
    public float rollAngle;

    public Rigidbody rb;                        //Rigidbody de l'avion


	void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate ()
    {
        //Inputs
        float roll = Input.GetAxis("Mouse X");
        float pitch = Input.GetAxis("Mouse Y");
        inputThrottle = Input.GetMouseButton(0);    //Acceleration

        Move(roll, pitch, inputThrottle);
	}

    public void Move(float rollInput, float pitchInput, bool ThrottleInput)
    {
        inputRoll = rollInput;
        inputPitch = pitchInput;
        inputThrottle = ThrottleInput;

        ClampInputs();
        CalculateRollPitchAngles();
        AutoLevel();
        ThrottleIncrease();
        CalculateFowardSpeed();
        ControlThrottle();
        CalculateLinearForces();
        CalculateTorque();
    }

    void ClampInputs()
    {
        //Bloque les inputs de -1 à 1
        //inputThrottle = Mathf.Clamp(inputThrottle, -1, 1);
        inputRoll = Mathf.Clamp(inputRoll, 0, 1);
        inputPitch = Mathf.Clamp(inputPitch, 0, 1);
        throttlePower = Mathf.Clamp(throttlePower, 0, 1);
    }

    void CalculateRollPitchAngles()
    {
        var flatForward = transform.forward;
        flatForward.y = 0;

        if (flatForward.sqrMagnitude > 0)
        {
            flatForward.Normalize();
            var localFlatForward = transform.InverseTransformDirection(flatForward);
            pitchAngle = Mathf.Atan2(localFlatForward.y, localFlatForward.z);

            var flatRight = Vector3.Cross(Vector3.up, flatForward);
            var localFlatRight = transform.InverseTransformDirection(flatRight);
            rollAngle = Mathf.Atan2(localFlatRight.y, localFlatRight.z);
        }
    }

    void AutoLevel()
    {
        bankedTurnAmount = Mathf.Sign(rollAngle);

        if (inputRoll == 0f)
            inputRoll = -rollAngle * autoRollLevel;

        if (inputPitch == 0f)
        {
            inputPitch = -pitchAngle * autoPitchLevel;
            inputPitch -= Mathf.Abs(bankedTurnAmount * bankedTurnAmount * autoTurnPitch);
        }
    }

    void ThrottleIncrease()
    {
        if (inputThrottle)
            throttlePower += Time.deltaTime;
        else
            throttlePower -= Time.deltaTime;
    }
    
    void CalculateFowardSpeed()
    {
        var localVelocity = transform.InverseTransformDirection(rb.velocity);
        speed = Mathf.Max(0, localVelocity.z);
    
    }

    void ControlThrottle()
    {
        //Si l'avion est immobile > overide des valeurs
        if (immobilized)
            throttlePower = -0.5f;

        //Ajustement des moteurs en fonctions des inputs
        throttle = Mathf.Clamp01(throttle + throttlePower * Time.deltaTime * throttleSpeed);

        //Modification de la puissance des moteurs
        enginePower = throttle * maxEnginePower;
    }

    void CalculateLinearForces()
    {
        //Calcul des forces sur l'avion
        var forces = Vector3.zero;

        //Ajout de la puissance des moteurs
        forces += enginePower * transform.forward;

        Debug.Log("forces avant lift = " + forces);

        var liftDirection = Vector3.Cross(rb.velocity, transform.right).normalized;
        var zeroLiftFactor = Mathf.InverseLerp(zeroLift, 0, speed);
        var liftPower = speed * speed * generatedLift * zeroLiftFactor * aeroFactor;
        forces += liftPower * liftDirection;

        Debug.Log("forces après lift = " + forces);

        rb.AddForce(forces);
    }

    void CalculateTorque()
    {
        var torque = Vector3.zero;

        torque += inputPitch * pitchEffect * transform.right;
        //Yaw effect
        torque += -inputRoll * rollEffect * transform.forward;
        torque += bankedTurnAmount * bankedTurnEffect * transform.up;

        rb.AddTorque(torque * speed * aeroFactor);
    }
}
