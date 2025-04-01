using UnityEngine;

public class BallThrow : MonoBehaviour
{
    public float launchForce = 10f;  // Fuerza para lanzar la pelota
    public float moveSpeed = 10f;  // Velocidad de movimiento de la pelota
    private Rigidbody rb;  // Referencia al Rigidbody de la pelota

    void Start()
    {
        // Obtener el Rigidbody al inicio
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Comprobar si el jugador presiona la tecla Espacio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Aplicar una fuerza hacia arriba
            rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);
        }

        // Obtener la entrada de las teclas W, A, S, D
        float moveHorizontal = Input.GetAxis("Horizontal");  // A, D
        float moveVertical = Input.GetAxis("Vertical");      // W, S

        // Calcular el movimiento en el espacio 3D
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);

        // Aplicar la fuerza de movimiento al Rigidbody
        rb.AddForce(movement * moveSpeed);
    }
}