using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AirplaneController : MonoBehaviour
{
    [SerializeField]
    float rollControlSensitivity = 0.2f;
    [SerializeField]
    float pitchControlSensitivity = 0.2f;
    [SerializeField]
    float yawControlSensitivity = 0.2f;
    [SerializeField]
    float thrustControlSensitivity = 0.01f;
    [SerializeField]
    float flapControlSensitivity = 0.15f;


    float pitch;
    float yaw;
    float roll;
    float flap;


    float thrustPercent;
    bool brake = false;
    bool permabrake = false;

    AircraftPhysics aircraftPhysics;
    Rotator propeller;

    private void Start()
    {
        aircraftPhysics = GetComponent<AircraftPhysics>();
        propeller = FindObjectOfType<Rotator>();
        SetThrust(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            SetThrust(thrustPercent + thrustControlSensitivity);
        }
        propeller.speed = thrustPercent * 1500f;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            thrustControlSensitivity *= -1;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            permabrake=!permabrake;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if(flap<60f)
            {
                flap += flapControlSensitivity;
                //clamp
                flap = Mathf.Clamp(flap, 0f, Mathf.Deg2Rad * 40);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if(flap>0f)
            {
                flap -= flapControlSensitivity;
                //clamp
                flap = Mathf.Clamp(flap, Mathf.Deg2Rad * -40, 0f);
            }
        }

        pitch = pitchControlSensitivity * Input.GetAxis("Vertical");
        roll = rollControlSensitivity * Input.GetAxis("Horizontal");
        yaw = yawControlSensitivity * Input.GetAxis("Yaw");
    }

    private void SetThrust(float percent)
    {
        thrustPercent = Mathf.Clamp01(percent);
    }

    private void FixedUpdate()
    {
        aircraftPhysics.SetControlSurfecesAngles(pitch, roll, yaw, flap);
        aircraftPhysics.SetThrustPercent(thrustPercent);
        aircraftPhysics.permaBrake(permabrake);
    }
}
