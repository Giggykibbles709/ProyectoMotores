using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;           // Referencia al coche del jugador
    public Vector3 offset = new Vector3(0, 5, -10); // Desplazamiento de la c�mara respecto al coche
    public float followSpeed = 10f;   // Velocidad de seguimiento de la c�mara
    public float rotationSpeed = 5f; // Velocidad de rotaci�n de la c�mara

    private Vector3 velocity = Vector3.zero; // Para suavizar el movimiento

    void LateUpdate()
    {
        // Calcula la posici�n deseada de la c�mara
        Vector3 targetPosition = player.position + player.TransformVector(offset);

        // Suaviza el movimiento de la c�mara hacia la posici�n objetivo
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 1f / followSpeed);

        // Calcula la rotaci�n deseada de la c�mara
        Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);

        // Suaviza la rotaci�n de la c�mara
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
