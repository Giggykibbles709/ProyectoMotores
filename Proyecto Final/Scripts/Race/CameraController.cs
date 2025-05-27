using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;           // Referencia al coche del jugador
    public Vector3 offset = new Vector3(0, 5, -10); // Desplazamiento de la cámara respecto al coche
    public float followSpeed = 10f;   // Velocidad de seguimiento de la cámara
    public float rotationSpeed = 5f; // Velocidad de rotación de la cámara

    private Vector3 velocity = Vector3.zero; // Para suavizar el movimiento

    void LateUpdate()
    {
        // Calcula la posición deseada de la cámara
        Vector3 targetPosition = player.position + player.TransformVector(offset);

        // Suaviza el movimiento de la cámara hacia la posición objetivo
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 1f / followSpeed);

        // Calcula la rotación deseada de la cámara
        Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);

        // Suaviza la rotación de la cámara
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
