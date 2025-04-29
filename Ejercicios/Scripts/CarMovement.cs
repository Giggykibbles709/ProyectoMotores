using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{

    public GameObject car; // Referencia al objeto del coche
    public GameObject[] wheels; // Array de ruedas

    public float maxCarRotationSpeed = 100f; // Velocidad m�xima de rotaci�n del coche
    public float maxWheelRotationSpeed = 100f; // Velocidad m�xima de rotaci�n de las ruedas
    public float acceleration = 2f; // Velocidad a la que aumenta la rotaci�n
    public float deceleration = 2f; // Velocidad a la que disminuye la rotaci�n

    private float carRotationSpeed = 0f; // Velocidad actual de rotaci�n del coche
    private float wheelRotationSpeed = 0f; // Velocidad actual de rotaci�n de las ruedas

    void Update()
    {
        // Control de rotaci�n para el coche
        if (Input.GetKey(KeyCode.R))
        {
            carRotationSpeed = Mathf.Lerp(carRotationSpeed, maxCarRotationSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            carRotationSpeed = Mathf.Lerp(carRotationSpeed, 0f, deceleration * Time.deltaTime);
        }
        car.transform.Rotate(Vector3.up * carRotationSpeed * Time.deltaTime);

        // Control de rotaci�n para las ruedas
        if (Input.GetKey(KeyCode.Q))
        {
            wheelRotationSpeed = Mathf.Lerp(wheelRotationSpeed, maxWheelRotationSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            wheelRotationSpeed = Mathf.Lerp(wheelRotationSpeed, 0f, deceleration * Time.deltaTime);
        }
        foreach (GameObject wheel in wheels)
        {
            wheel.transform.Rotate(Vector3.right * wheelRotationSpeed * Time.deltaTime);
        }
    }
}
