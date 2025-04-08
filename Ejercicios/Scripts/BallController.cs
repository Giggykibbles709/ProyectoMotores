using UnityEngine;

public class BallController : MonoBehaviour
{
    // Referencia al componente Rigidbody del bal�n
    private Rigidbody rb;

    // Fuerza aplicada al tirar
    public float kickForce = 5f;

    // Fuerza aplicada para levantar el bal�n hacia arriba
    public float liftForce = 5f;

    // Posici�n inicial del bal�n para poder reiniciarlo
    private Vector3 startPosition;

    // Lista de jugadores que pueden interactuar con el bal�n
    public PlayerInputHandler[] players;

    void Start()
    {
        // Inicializamos el Rigidbody y guardamos la posici�n inicial del bal�n
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    void Update()
    {
        // Iteramos por cada jugador para verificar si est� cerca del bal�n
        foreach (var player in players)
        {
            if (player.IsNearBall(transform.position)) // Comprobamos si el jugador est� cerca del bal�n
            {
                if (player.TryKick(out Vector3 direction)) // Intentamos tirar
                {
                    // Aplicamos una fuerza en la direcci�n del lanzamiento
                    rb.AddForce(direction * kickForce, ForceMode.Impulse);
                }
                if (player.TryLift()) // Intentamos levantar el bal�n
                {
                    // Aplicamos una fuerza hacia arriba
                    rb.AddForce(Vector3.up * liftForce, ForceMode.Impulse);
                }
            }
        }
    }

    // M�todo para reiniciar la posici�n y velocidad del bal�n
    public void ResetBall()
    {
        // Detenemos el movimiento y la rotaci�n del bal�n
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Colocamos el bal�n en su posici�n inicial
        transform.position = startPosition;
    }
}
