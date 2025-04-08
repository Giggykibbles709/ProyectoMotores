using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    // Referencia al Rigidbody del jugador para aplicar movimiento y fuerzas
    public Rigidbody rb;

    // Velocidad de movimiento del jugador
    public float moveSpeed = 5f;

    // Fuerza del salto del jugador
    public float jumpForce = 5f;

    // Distancia m�xima para interactuar con el bal�n
    public float interactionDistance = 3f;

    // Variables para almacenar los inputs del jugador
    private Vector2 moveInput;   // Direcci�n de movimiento (X, Y)
    private bool jumpPressed;    // Si se presion� el bot�n de salto
    private bool kickPressed;    // Si se presion� el bot�n de tirar
    private bool liftPressed;    // Si se presion� el bot�n de levantar

    // Sistema de entrada de Unity para gestionar los controles
    private PlayerInput playerInput;

    // Referencia a la c�mara del jugador para calcular direcciones de movimiento
    public Camera playerCamera;

    void Awake()
    {
        // Inicializamos el componente PlayerInput
        playerInput = GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        // Configuramos las acciones del sistema de entrada
        var actions = playerInput.actions;

        // Asignar el movimiento
        actions["Move"].performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        actions["Move"].canceled += ctx => moveInput = Vector2.zero;

        // Asignar el salto
        actions["Jump"].performed += _ => jumpPressed = true;

        // Asignar el tiro
        actions["Kick"].performed += _ => kickPressed = true;

        // Asignar el levantamiento
        actions["Lift"].performed += _ => liftPressed = true;
    }

    void Update()
    {
        if (playerCamera == null)
        {
            // Mostramos un error si no se asigna una c�mara al jugador
            Debug.LogError("Player Camera is not assigned!");
            return;
        }

        // Calcular el movimiento basado en la orientaci�n de la c�mara
        Vector3 forward = playerCamera.transform.forward; // Direcci�n hacia adelante
        Vector3 right = playerCamera.transform.right;     // Direcci�n hacia la derecha

        // Ignorar la inclinaci�n vertical de la c�mara
        forward.y = 0;
        right.y = 0;

        // Normalizar las direcciones para obtener magnitudes consistentes
        forward.Normalize();
        right.Normalize();

        // Combinar los inputs del jugador con las direcciones de la c�mara
        Vector3 move = (forward * moveInput.y + right * moveInput.x).normalized;

        // Aplicar movimiento al jugador
        rb.MovePosition(transform.position + move * moveSpeed * Time.deltaTime);

        // Aplicar salto si el bot�n fue presionado
        if (jumpPressed)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpPressed = false; // Reseteamos el estado del salto
        }
    }

    // M�todo para intentar tirar
    public bool TryKick(out Vector3 direction)
    {
        if (kickPressed)
        {
            // Calculamos la direcci�n basada en la c�mara del jugador
            direction = (playerCamera.transform.forward + Vector3.up * 0.1f).normalized;
            kickPressed = false; // Reseteamos el estado del tiro
            return true;
        }
        direction = Vector3.zero; // Si no se presion�, la direcci�n es nula
        return false;
    }

    // M�todo para intentar levantar el bal�n
    public bool TryLift()
    {
        if (liftPressed)
        {
            liftPressed = false; // Reseteamos el estado de levantamiento
            return true;
        }
        return false;
    }

    // M�todo para verificar si el jugador est� cerca del bal�n
    public bool IsNearBall(Vector3 ballPosition)
    {
        // Calculamos la distancia entre el jugador y el bal�n
        return Vector3.Distance(transform.position, ballPosition) <= interactionDistance;
    }
}
