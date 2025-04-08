using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Desplazamiento entre la posición inicial de la cámara y el objetivo
    private Vector3 offset;

    // Transform del objetivo a seguir (en este caso, el jugador)
    public Transform target;

    // Tiempo para suavizar el movimiento de la cámara
    public float smoothTime;

    // Velocidad actual utilizada por el método SmoothDamp
    private Vector3 currentVelocity = Vector3.zero;

    private void Awake()
    {
        // Calcula el desplazamiento inicial entre la cámara y el objetivo
        offset = transform.position - target.position;
    }

    // LateUpdate se utiliza para garantizar que la cámara siga al objetivo
    // después de que este haya completado su movimiento en el frame actual.
    private void LateUpdate()
    {
        // Verifica si el objetivo está asignado
        if (target != null)
        {
            // Calcula la posición deseada de la cámara en base a la posición del objetivo y el desplazamiento
            Vector3 targetPosition = target.position + offset;

            // Suaviza el movimiento de la cámara hacia la posición objetivo
            transform.position = Vector3.SmoothDamp(
                current: transform.position, // Posición actual de la cámara
                target: targetPosition,      // Posición deseada
                currentVelocity: ref currentVelocity, // Referencia a la velocidad actual
                smoothTime: smoothTime       // Tiempo de suavizado
            );
        }
    }
}