using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;           // Referencia al coche del jugador
    public Vector3 offset = new Vector3(0, 5, -10); // Desplazamiento de la c�mara respecto al coche
    public float followSpeed = 10f;   // Velocidad de seguimiento de la c�mara
    public float rotationSpeed = 5f; // Velocidad de rotaci�n de la c�mara

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
            Debug.LogError("La referencia al jugador no est� asignada en CameraController.");
        }
    }

    private void LateUpdate()
    {
        if (car == null) return; // Salir si no hay referencia al coche

        // Calcula la posici�n deseada de la c�mara
        Vector3 targetPosition = car.position + car.TransformVector(offset);

        // Suaviza el movimiento de la c�mara hacia la posici�n objetivo
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 1f / followSpeed);

        // Calcula la rotaci�n deseada de la c�mara
        Quaternion targetRotation = Quaternion.LookRotation(car.position - transform.position);

        // Suaviza la rotaci�n de la c�mara
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
