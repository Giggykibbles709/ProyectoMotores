using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;           // Referencia al coche del jugador
    public Vector3 offset = new Vector3(0, 5, -10); // Desplazamiento de la cámara respecto al coche
    public float followSpeed = 10f;   // Velocidad de seguimiento de la cámara
    public float rotationSpeed = 5f; // Velocidad de rotación de la cámara

    private Transform car; // Referencia al coche como hijo del player
    private Vector3 velocity = Vector3.zero; // Para suavizar el movimiento

    private void Start()
    {
        if (player != null)
        {
            // Busca el primer hijo del player como referencia al coche
            car = player.GetChild(0);
        }
        else
        {
            Debug.LogError("La referencia al jugador no está asignada en CameraController.");
        }
    }

    private void LateUpdate()
    {
        if (car == null) return; // Salir si no hay referencia al coche

        // Calcula la posición deseada de la cámara
        Vector3 targetPosition = car.position + car.TransformVector(offset);

        // Suaviza el movimiento de la cámara hacia la posición objetivo
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 1f / followSpeed);

        // Calcula la rotación deseada de la cámara
        Quaternion targetRotation = Quaternion.LookRotation(car.position - transform.position);

        // Suaviza la rotación de la cámara
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
