using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    private Rigidbody playerRb;
    public Vector3 offset;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = player.GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        Vector3 playerForward = (playerRb.velocity + player.transform.forward).normalized;
        transform.position = Vector3.Lerp(transform.position, player.position + player.transform.TransformVector(offset) + playerForward * (-5f), speed * Time.deltaTime);
        transform.LookAt(player);
    }
}
