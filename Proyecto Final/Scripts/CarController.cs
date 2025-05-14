using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;

    // Configuraciones del coche
    [SerializeField] private float motorForce, breakForce, maxSteerAngle, skidThreshold;

    // Colliders de las ruedas
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Transformaciones de las ruedas (para sincronizar la posición visual)
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    // Sistemas de partículas para el humo
    [SerializeField] private ParticleSystem frontLeftSmoke, frontRightSmoke;
    [SerializeField] private ParticleSystem rearLeftSmoke, rearRightSmoke;

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        HandleSmoke();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        verticalInput = Input.GetAxis("Vertical");

        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        // Aplica fuerza motriz a las ruedas delanteras
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;

        // Determina la fuerza de frenado
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        // Aplica la fuerza de frenado a todas las ruedas
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        // Calcula el ángulo de giro basado en el input horizontal
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        // Actualiza la posición y rotación visual de cada rueda
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        // Sincroniza la posición y rotación del objeto visual de la rueda con el collider
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void HandleSmoke()
    {
        // Activa/desactiva las partículas de humo para las ruedas
        HandleWheelSmoke(frontLeftWheelCollider, frontLeftSmoke);
        HandleWheelSmoke(frontRightWheelCollider, frontRightSmoke);
        HandleWheelSmoke(rearLeftWheelCollider, rearLeftSmoke);
        HandleWheelSmoke(rearRightWheelCollider, rearRightSmoke);
    }

    private void HandleWheelSmoke(WheelCollider wheelCollider, ParticleSystem smoke)
    {
        WheelHit wheelHit;
        if (wheelCollider.GetGroundHit(out wheelHit))
        {
            // Calcula si la rueda está derrapando
            bool isSkidding = Mathf.Abs(wheelHit.sidewaysSlip) > skidThreshold;

            // Calcula si se está haciendo un burnout (acelerar + frenar)
            bool isBurnout = isBreaking && verticalInput > 0 && Mathf.Abs(wheelHit.forwardSlip) > skidThreshold;

            // Activa las partículas si hay derrape o burnout
            if (isSkidding || isBurnout)
            {
                if (!smoke.isPlaying)
                    smoke.Play();
            }
            else
            {
                // Detiene las partículas si no hay derrape ni burnout
                if (smoke.isPlaying)
                    smoke.Stop();
            }
        }
    }
}