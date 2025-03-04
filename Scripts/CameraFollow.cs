using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset;
    public Transform target;
    public float smoothTime;
    private Vector3 currentVelocity = Vector3.zero;

    private void Awake()
    {
        offset = transform.position - target.position;  // Cálculo de la distancia entre la cámara y el objetivo
    }

    // Usamos LateUpdate para suavizar movimientos bruscos del jugador
    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset; // Cálculo de la posición del objetivo
            transform.position = Vector3.SmoothDamp(current: transform.position, targetPosition, ref currentVelocity, smoothTime); // Movimiento suave de la cámara mientras sigue al objetivo
        }
    }
}