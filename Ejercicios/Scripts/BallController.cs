using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;
    public float clickForce = 5f;
    public float liftForce = 5f;

    private Vector3 startPosition;
    public Transform player; // Referencia al jugador
    public float interactionDistance = 3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Click izquierdo - disparar balón SOLO si está cerca
            if (Vector3.Distance(player.position, transform.position) <= interactionDistance)
            {
                Vector3 direction = (transform.position - Camera.main.transform.position).normalized;
                rb.AddForce(direction * clickForce, ForceMode.Impulse);
            }
        }
        // Click derecho - levantar balón
        if (Input.GetMouseButtonDown(1)) // 1 = botón derecho
        {
            if (Vector3.Distance(player.position, transform.position) <= interactionDistance)
            {
                rb.AddForce(Vector3.up * liftForce, ForceMode.Impulse);
            }
        }
    }

    public void ResetBall()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = startPosition;
    }
}
