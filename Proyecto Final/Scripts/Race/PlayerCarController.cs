using UnityEngine;

public class PlayerCarController : MonoBehaviour
{
    [SerializeField] private float accelerationForce = 20f;  // Fuerza de aceleraci�n
    [SerializeField] private float maxSpeed = 50f;          // Velocidad m�xima
    [SerializeField] private float turnSpeed = 50f;         // Velocidad de giro
    [SerializeField] private float drag = 0.99f;            // Simulaci�n de resistencia al movimiento

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    private Rigidbody rb;

    private void Start()
    {
        // Obtener el Rigidbody del coche
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleSteering();
        UpdateWheels();
    }

    private void HandleMovement()
    {
        // Obtener el input vertical para avanzar o retroceder
        float forwardInput = Input.GetAxis("Vertical");

        // Aplicar fuerza en la direcci�n del coche
        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * forwardInput * accelerationForce, ForceMode.Acceleration);
        }

        // Simular resistencia al movimiento
        rb.velocity *= drag;
    }

    private void HandleSteering()
    {
        // Obtener el input horizontal para girar
        float turnInput = Input.GetAxis("Horizontal");

        // Aplicar rotaci�n para girar el coche
        transform.Rotate(0f, turnInput * turnSpeed * Time.fixedDeltaTime, 0f);
    }

    private void UpdateWheels()
    {
        // Rotar las ruedas en el eje X para simular el avance
        float rotationAmount = rb.velocity.magnitude * Time.fixedDeltaTime * Mathf.Sign(Vector3.Dot(rb.velocity, transform.forward));
        frontLeftWheelTransform.Rotate(Vector3.right, rotationAmount);
        frontRightWheelTransform.Rotate(Vector3.right, rotationAmount);
        rearLeftWheelTransform.Rotate(Vector3.right, rotationAmount);
        rearRightWheelTransform.Rotate(Vector3.right, rotationAmount);

        // Ajustar la direcci�n de las ruedas delanteras en el eje Y para simular el giro
        float turnInput = Input.GetAxis("Horizontal");
        float maxSteerAngle = 30f; // �ngulo m�ximo de direcci�n
        Quaternion steerRotation = Quaternion.Euler(0f, turnInput * maxSteerAngle, 0f);

        frontLeftWheelTransform.localRotation = steerRotation;
        frontRightWheelTransform.localRotation = steerRotation;
    }
}
