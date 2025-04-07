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
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
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
            direction = (Camera.main.transform.forward + Vector3.up * 0.1f).normalized;
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
