using UnityEngine;

public class SimpleCarPhysics : MonoBehaviour
{
    [SerializeField] private float accelerationForce = 20f;  // Fuerza de aceleración
    [SerializeField] private float maxSpeed = 50f;          // Velocidad máxima
    [SerializeField] private float turnSpeed = 50f;         // Velocidad de giro
    [SerializeField] private float drag = 0.99f;            // Simulación de resistencia al movimiento

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
    }

    private void HandleMovement()
    {
        // Obtener el input vertical para avanzar o retroceder
        float forwardInput = Input.GetAxis("Vertical");

        // Aplicar fuerza en la dirección del coche
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

        // Aplicar rotación para girar el coche
        transform.Rotate(0f, turnInput * turnSpeed * Time.fixedDeltaTime, 0f);
    }
}
