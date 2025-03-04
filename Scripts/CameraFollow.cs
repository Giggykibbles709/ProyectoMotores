using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset;
    public Transform target;
    public float smoothTime;
    private Vector3 currentVelocity = Vector3.zero;

    private void Awake()
    {
        offset = transform.position - target.position;  // C�lculo de la distancia entre la c�mara y el objetivo
    }

    // Usamos LateUpdate para suavizar movimientos bruscos del jugador
    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset; // C�lculo de la posici�n del objetivo
            transform.position = Vector3.SmoothDamp(current: transform.position, targetPosition, ref currentVelocity, smoothTime); // Movimiento suave de la c�mara mientras sigue al objetivo
        }
    }
}