using UnityEngine;

public class BallController : MonoBehaviour
{
    // Referencia al componente Rigidbody del balón
    private Rigidbody rb;

    // Fuerza aplicada al tirar
    public float kickForce = 5f;

    // Fuerza aplicada para levantar el balón hacia arriba
    public float liftForce = 5f;

    // Posición inicial del balón para poder reiniciarlo
    private Vector3 startPosition;

    // Lista de jugadores que pueden interactuar con el balón
    public PlayerInputHandler[] players;

    void Start()
    {
        // Inicializamos el Rigidbody y guardamos la posición inicial del balón
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    void Update()
    {
        // Iteramos por cada jugador para verificar si está cerca del balón
        foreach (var player in players)
        {
            if (player.IsNearBall(transform.position)) // Comprobamos si el jugador está cerca del balón
            {
                if (player.TryKick(out Vector3 direction)) // Intentamos tirar
                {
                    // Aplicamos una fuerza en la dirección del lanzamiento
                    rb.AddForce(direction * kickForce, ForceMode.Impulse);
                }
                if (player.TryLift()) // Intentamos levantar el balón
                {
                    // Aplicamos una fuerza hacia arriba
                    rb.AddForce(Vector3.up * liftForce, ForceMode.Impulse);
                }
            }
        }
    }

    // Método para reiniciar la posición y velocidad del balón
    public void ResetBall()
    {
        // Detenemos el movimiento y la rotación del balón
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Colocamos el balón en su posición inicial
        transform.position = startPosition;
    }
}
