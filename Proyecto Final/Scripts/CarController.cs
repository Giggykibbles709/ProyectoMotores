using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Rigidbody playerRb;
    public WheelColliders colliders;
    public WheelMeshes wheelMeshes;
    public WheelParticles wheelParticles;
    public float gasInput;
    public float brakeInput;
    public float steeringInput;
    public GameObject smokePrefab;
    public float motorPower;
    public float brakePower;
    private float slipAngle;
    private float speed;
    public AnimationCurve steeringCurve;

    private void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
        InstantiateSmoke();
    }

    void InstantiateSmoke()
    {
        wheelParticles.frontRightWheel = Instantiate(smokePrefab, colliders.frontRightWheel.transform.position - Vector3.up * colliders.frontRightWheel.radius, Quaternion.identity, colliders.frontRightWheel.transform).GetComponent<ParticleSystem>();
        wheelParticles.frontLeftWheel = Instantiate(smokePrefab, colliders.frontLeftWheel.transform.position - Vector3.up * colliders.frontLeftWheel.radius, Quaternion.identity, colliders.frontLeftWheel.transform).GetComponent<ParticleSystem>();
        wheelParticles.rearRightWheel = Instantiate(smokePrefab, colliders.rearRightWheel.transform.position - Vector3.up * colliders.rearRightWheel.radius, Quaternion.identity, colliders.rearRightWheel.transform).GetComponent<ParticleSystem>();
        wheelParticles.rearLeftWheel = Instantiate(smokePrefab, colliders.rearLeftWheel.transform.position - Vector3.up * colliders.rearLeftWheel.radius, Quaternion.identity, colliders.rearLeftWheel.transform).GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        speed = playerRb.velocity.magnitude;
        CheckInput();
        ApplyMotor();
        ApplySteering();
        ApplyBrake();
        CheckParticles();
        ApplyWheelPosition();
    }

    void CheckInput()
    {
        gasInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");
        slipAngle = Vector3.Angle(transform.forward, playerRb.velocity-transform.forward);
        if(slipAngle < 120f)
        {
            if(gasInput < 0)
            {
                brakeInput = Mathf.Abs(gasInput);
                gasInput = 0;
            }
        }
        else
        {
            brakeInput = 0;
        }
    }

    void ApplyBrake()
    {
        colliders.frontRightWheel.brakeTorque = brakeInput * brakePower * 0.7f;
        colliders.frontLeftWheel.brakeTorque = brakeInput * brakePower * 0.7f;
        colliders.rearRightWheel.brakeTorque = brakeInput * brakePower * 0.3f;
        colliders.rearLeftWheel.brakeTorque = brakeInput * brakePower * 0.3f;
    }

    void ApplyMotor()
    {
        colliders.rearRightWheel.motorTorque = motorPower * gasInput;
        colliders.rearLeftWheel.motorTorque = motorPower * gasInput;
    }

    void ApplySteering()
    {
        float steeringAngle = steeringInput * steeringCurve.Evaluate(speed);
        steeringAngle += Vector3.SignedAngle(transform.forward, playerRb.velocity + transform.forward, Vector3.up);
        steeringAngle = Mathf.Clamp(steeringAngle, -90f, 90f);
        colliders.frontRightWheel.steerAngle = steeringAngle;
        colliders.frontLeftWheel.steerAngle = steeringAngle;
    }

    void ApplyWheelPosition()
    {
        UpdateWheel(colliders.frontRightWheel, wheelMeshes.frontRightWheel);
        UpdateWheel(colliders.frontLeftWheel, wheelMeshes.frontLeftWheel);
        UpdateWheel(colliders.rearRightWheel, wheelMeshes.rearRightWheel);
        UpdateWheel(colliders.rearLeftWheel, wheelMeshes.rearLeftWheel);
    }

    void CheckParticles()
    {
        WheelHit[] wheelHits = new WheelHit[4];
        colliders.frontRightWheel.GetGroundHit(out wheelHits[0]);
        colliders.frontLeftWheel.GetGroundHit(out wheelHits[1]);
        colliders.rearRightWheel.GetGroundHit(out wheelHits[2]);
        colliders.rearLeftWheel.GetGroundHit(out wheelHits[3]);

        float slipAllowance = 0.5f;
        if ((Mathf.Abs(wheelHits[0].sidewaysSlip) + Mathf.Abs(wheelHits[0].forwardSlip) > slipAllowance))
        {
            wheelParticles.frontRightWheel.Play();
        }
        else
        {
            wheelParticles.frontRightWheel.Stop();
        }
        if ((Mathf.Abs(wheelHits[1].sidewaysSlip) + Mathf.Abs(wheelHits[1].forwardSlip) > slipAllowance))
        {
            wheelParticles.frontLeftWheel.Play();
        }
        else
        {
            wheelParticles.frontLeftWheel.Stop();
        }
        if ((Mathf.Abs(wheelHits[2].sidewaysSlip) + Mathf.Abs(wheelHits[2].forwardSlip) > slipAllowance))
        {
            wheelParticles.rearRightWheel.Play();
        }
        else
        {
            wheelParticles.rearRightWheel.Stop();
        }
        if ((Mathf.Abs(wheelHits[3].sidewaysSlip) + Mathf.Abs(wheelHits[3].forwardSlip) > slipAllowance))
        {
            wheelParticles.rearLeftWheel.Play();
        }
        else
        {
            wheelParticles.rearLeftWheel.Stop();
        }
    }

    void UpdateWheel(WheelCollider collider, MeshRenderer mesh)
    {
        Quaternion quat;
        Vector3 position;
        collider.GetWorldPose(out position, out quat);
        mesh.transform.position = position;
        mesh.transform.rotation = quat;
    }
}

[System.Serializable]
public class WheelColliders
{
    public WheelCollider frontRightWheel;
    public WheelCollider frontLeftWheel;
    public WheelCollider rearRightWheel;
    public WheelCollider rearLeftWheel;
}

[System.Serializable]
public class WheelMeshes
{
    public MeshRenderer frontRightWheel;
    public MeshRenderer frontLeftWheel;
    public MeshRenderer rearRightWheel;
    public MeshRenderer rearLeftWheel;
}

[System.Serializable]
public class WheelParticles
{
    public ParticleSystem frontRightWheel;
    public ParticleSystem frontLeftWheel;
    public ParticleSystem rearRightWheel;
    public ParticleSystem rearLeftWheel;
}