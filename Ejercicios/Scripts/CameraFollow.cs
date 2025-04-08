using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Desplazamiento entre la posici�n inicial de la c�mara y el objetivo
    private Vector3 offset;

    // Transform del objetivo a seguir (en este caso, el jugador)
    public Transform target;

    // Tiempo para suavizar el movimiento de la c�mara
    public float smoothTime;

    // Velocidad actual utilizada por el m�todo SmoothDamp
    private Vector3 currentVelocity = Vector3.zero;

    private void Awake()
    {
        // Calcula el desplazamiento inicial entre la c�mara y el objetivo
        offset = transform.position - target.position;
    }

    // LateUpdate se utiliza para garantizar que la c�mara siga al objetivo
    // despu�s de que este haya completado su movimiento en el frame actual.
    private void LateUpdate()
    {
        // Verifica si el objetivo est� asignado
        if (target != null)
        {
            // Calcula la posici�n deseada de la c�mara en base a la posici�n del objetivo y el desplazamiento
            Vector3 targetPosition = target.position + offset;

            // Suaviza el movimiento de la c�mara hacia la posici�n objetivo
            transform.position = Vector3.SmoothDamp(
                current: transform.position, // Posici�n actual de la c�mara
                target: targetPosition,      // Posici�n deseada
                currentVelocity: ref currentVelocity, // Referencia a la velocidad actual
                smoothTime: smoothTime       // Tiempo de suavizado
            );
        }
    }
}