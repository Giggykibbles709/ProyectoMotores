using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;
    public float kickForce = 5f;
    public float liftForce = 5f;

    private Vector3 startPosition;
    public PlayerInputHandler[] players;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    void Update()
    {
        foreach (var player in players)
        {
            if (player.IsNearBall(transform.position))
            {
                if (player.TryKick(out Vector3 direction))
                {
                    rb.AddForce(direction * kickForce, ForceMode.Impulse);
                }
                if (player.TryLift())
                {
                    rb.AddForce(Vector3.up * liftForce, ForceMode.Impulse);
                }
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
