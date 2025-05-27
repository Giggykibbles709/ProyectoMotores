using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float accelerationForce = 20f;   // Fuerza de aceleraci�n
    [SerializeField] private float maxSpeed = 50f;           // Velocidad m�xima
    [SerializeField] private float turnSpeed = 50f;          // Velocidad de giro
    [SerializeField] private float drag = 0.99f;             // Simulaci�n de resistencia al movimiento
    [SerializeField] private float decelerationFactor = 5f;  // Factor de desaceleraci�n progresiva

    private Rigidbody rb;

    private void Start()
    {
        // Obtener el Rigidbody del coche
        rb = GetComponent<Rigidbody>();
        rb.drag = 0f; // Desactivar drag f�sico predeterminado para usar nuestro propio sistema
        rb.angularDrag = 0f;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleSteering();
        ApplyDeceleration();
    }

    private void HandleMovement()
    {
        // Obtener el input vertical para avanzar o retroceder
        float forwardInput = Input.GetAxis("Vertical");

        // Aplicar fuerza en la direcci�n del coche solo si hay input significativo
        if (Mathf.Abs(forwardInput) > 0.1f)
        {
            Vector3 force = transform.forward * forwardInput * accelerationForce;

            // Limitar la velocidad m�xima antes de aplicar la fuerza
            if (rb.velocity.magnitude < maxSpeed)
            {
                rb.AddForce(force, ForceMode.Acceleration);
            }
        }
    }

    private void HandleSteering()
    {
        // Obtener el input horizontal para girar
        float turnInput = Input.GetAxis("Horizontal");

        // Solo permitir giro si el coche est� en movimiento
        if (rb.velocity.magnitude > 0.1f)
        {
            float turnAngle = turnInput * turnSpeed * Time.fixedDeltaTime;
            transform.Rotate(0f, turnAngle, 0f);
        }
    }

    private void ApplyDeceleration()
    {
        // Si no hay entrada de aceleraci�n, desacelerar el coche gradualmente
        if (Input.GetAxis("Vertical") == 0f)
        {
            Vector3 deceleration = rb.velocity * -decelerationFactor * Time.fixedDeltaTime;
            rb.AddForce(deceleration, ForceMode.VelocityChange);
        }

        // Aplicar resistencia al movimiento (drag)
        rb.velocity *= drag;

        // Limitar la velocidad a cero si es muy baja para evitar movimientos no deseados
        if (rb.velocity.magnitude < 0.1f)
        {
            rb.velocity = Vector3.zero;
        }
    }
}
