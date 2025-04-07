using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Rigidbody rb;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    public float interactionDistance = 3f;

    private Vector2 moveInput;
    private bool jumpPressed;
    private bool kickPressed;
    private bool liftPressed;

    private PlayerInput playerInput;
    public Camera playerCamera;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void OnEnable()
    {
        var actions = playerInput.actions;

        actions["Move"].performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        actions["Move"].canceled += ctx => moveInput = Vector2.zero;

        actions["Jump"].performed += _ => jumpPressed = true;
        actions["Kick"].performed += _ => kickPressed = true;
        actions["Lift"].performed += _ => liftPressed = true;
    }

    void Update()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera is not assigned!");
            return;
        }

        // Calcular el movimiento basado en la orientación de la cámara
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        forward.y = 0; // Ignorar inclinación vertical de la cámara
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 move = (forward * moveInput.y + right * moveInput.x).normalized;
        rb.MovePosition(transform.position + move * moveSpeed * Time.deltaTime);

        if (jumpPressed)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpPressed = false;
        }
    }

    public bool TryKick(out Vector3 direction)
    {
        if (kickPressed)
        {
            // Disparo basado en la orientación de la cámara del jugador
            direction = (playerCamera.transform.forward + Vector3.up * 0.1f).normalized;
            kickPressed = false;
            return true;
        }
        direction = Vector3.zero;
        return false;
    }

    public bool TryLift()
    {
        if (liftPressed)
        {
            liftPressed = false;
            return true;
        }
        return false;
    }

    public bool IsNearBall(Vector3 ballPosition)
    {
        return Vector3.Distance(transform.position, ballPosition) <= interactionDistance;
    }
}
