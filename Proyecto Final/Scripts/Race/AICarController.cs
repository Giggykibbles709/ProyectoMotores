using UnityEngine;

public class AICarController : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] waypoints;
    public float waypointDistanceThreshold = 2f;

    [Header("Movement Settings")]
    public float baseSpeed;
    public float turnSpeed = 5f;
    private float currentSpeed;

    [Header("Collision Avoidance")]
    public float detectionRadius = 1.5f;
    public float detectionRange = 5f;
    public float avoidTurnStrength = 20f;

    [Header("Overtaking Settings")]
    public float overtakingRange = 3f;
    public float slowDownFactor = 0.5f;
    public float overtakingAggressiveness = 0.5f;

    [Header("Wheel Transforms")]
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    private int currentWaypointIndex = 0;

    void Start()
    {
        // Asignar una velocidad máxima única para cada coche.
        baseSpeed = Random.Range(8f, 20f);
        currentSpeed = baseSpeed;
        overtakingAggressiveness = Random.Range(0.2f, 1f);
    }

    void Update()
    {
        // Verifica que existan waypoints asignados.
        if (waypoints == null || waypoints.Length == 0) return;

        // Detectar obstáculos y evitar colisiones.
        AvoidCollisions();

        // Seguir la ruta de waypoints.
        FollowWaypoints();

        // Actualizar las ruedas visualmente.
        UpdateWheels();
    }

    private void FollowWaypoints()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 targetPosition = targetWaypoint.position;

        // Añadir una desviación lateral al objetivo.
        targetPosition.x += Random.Range(-1f, 1f);

        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 moveDirection = Vector3.Lerp(transform.forward, direction, Time.deltaTime * turnSpeed);

        transform.forward = moveDirection;
        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetWaypoint.position) < waypointDistanceThreshold)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    private void AvoidCollisions()
    {
        RaycastHit hit;

        // Detectar obstáculos con un SphereCast.
        if (Physics.SphereCast(transform.position, detectionRadius, transform.forward, out hit, detectionRange))
        {
            if (hit.collider.CompareTag("AICar") || hit.collider.CompareTag("Player"))
            {
                // Intentar adelantar hacia un lado.
                Vector3 overtakingDirection = Vector3.zero;

                // Detectar si puede adelantar por la derecha.
                if (!Physics.Raycast(transform.position, transform.right, overtakingRange))
                {
                    overtakingDirection = transform.right;
                }
                // Detectar si puede adelantar por la izquierda.
                else if (!Physics.Raycast(transform.position, -transform.right, overtakingRange))
                {
                    overtakingDirection = -transform.right;
                }

                // Si hay espacio para adelantar, moverse lateralmente.
                if (overtakingDirection != Vector3.zero)
                {
                    Vector3 newPosition = transform.position + overtakingDirection * Time.deltaTime * avoidTurnStrength;
                    transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime);

                    // Mantener la velocidad base al intentar adelantar.
                    currentSpeed = baseSpeed;
                }
                else
                {
                    // Si no puede adelantar, alinearse detrás del coche de adelante.
                    Vector3 directionToTarget = (hit.collider.transform.position - transform.position).normalized;

                    // Ajustar la dirección del coche hacia el coche de adelante.
                    transform.forward = Vector3.Lerp(transform.forward, directionToTarget, Time.deltaTime * turnSpeed);

                    // Mantenerse a la misma velocidad que el coche de adelante.
                    currentSpeed = Mathf.Lerp(currentSpeed, baseSpeed, Time.deltaTime);
                }
            }
        }
        else
        {
            // Restaurar la velocidad si no hay obstáculos.
            currentSpeed = Mathf.Lerp(currentSpeed, baseSpeed, Time.deltaTime);
        }

        // Debug visual para verificar el SphereCast y los intentos de adelantamiento.
        Debug.DrawRay(transform.position, transform.forward * detectionRange, Color.red);
        Debug.DrawRay(transform.position, transform.right * overtakingRange, Color.blue);
        Debug.DrawRay(transform.position, -transform.right * overtakingRange, Color.green);
    }

    private void UpdateWheels()
    {
        // Rotar las ruedas en el eje X para simular el avance
        float rotationAmount = currentSpeed * Time.deltaTime;
        frontLeftWheelTransform.Rotate(Vector3.right, rotationAmount);
        frontRightWheelTransform.Rotate(Vector3.right, rotationAmount);
        rearLeftWheelTransform.Rotate(Vector3.right, rotationAmount);
        rearRightWheelTransform.Rotate(Vector3.right, rotationAmount);

        // Ajustar la dirección de las ruedas delanteras en el eje Y para simular el giro
        float maxSteerAngle = 30f; // Ángulo máximo de dirección
        Vector3 localEulerAngles = frontLeftWheelTransform.localEulerAngles;

        frontLeftWheelTransform.localEulerAngles = new Vector3(localEulerAngles.x, Mathf.Sin(Time.time * turnSpeed) * maxSteerAngle, localEulerAngles.z);
        frontRightWheelTransform.localEulerAngles = frontLeftWheelTransform.localEulerAngles;
    }
}
