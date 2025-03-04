using UnityEngine;

public class CubeGameLoop : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    public float rotateSpeed = 50f; // Velocidad de rotación
    private bool isManualRotation = false; // Bool para controlar la rotación manual

    void Update()
    {
        // Movimiento hacia adelante y atrás con las teclas W y S.
        // Usamos Time.deltaTime para garantizar que el movimiento sea consistente en todos los dispositivos,
        // independientemente de los FPS (frames por segundo) del sistema.
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(0, 0, move);

        // Rotación con las teclas A y D.
        // Usamos Time.deltaTime para que la velocidad de rotación sea consistente.
        float rotate = Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;
        //Usamos Mathf.Abs para devolver el valor absoluto del input del jugador.
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01f) // Detectar si hay input en el eje horizontal
        {
            isManualRotation = true; // Activar bool de rotación manual
            transform.Rotate(0, rotate, 0);
        }
        else
        {
            isManualRotation = false; // No hay input, volver a rotación automática
        }
    }

    void FixedUpdate()
    {
        // Rotación automática constante en FixedUpdate.
        // Aquí usamos Time.fixedDeltaTime para garantizar que la rotación sea consistente con los intervalos fijos de FixedUpdate.

        // Si no hay rotación manual, continuar con la rotación automática.
        if (!isManualRotation)
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.fixedDeltaTime);
        }

        // FixedUpdate es útil para operaciones relacionadas con físicas, ya que se ejecuta a intervalos regulares,
        // independientemente de la velocidad de los frames.
    }
}

/*
Preguntas:

1. ¿Por qué es necesario Time.deltaTime? ¿Qué sucede si eliminas Time.deltaTime?
   - Time.deltaTime representa el tiempo que ha pasado desde el último cuadro. 
     Sin él, las operaciones de movimiento y rotación dependerían de los FPS, lo que 
     provocaría que el objeto se mueva más rápido en sistemas con más FPS y más lento en sistemas con menos FPS.
   - Usar Time.deltaTime asegura que el movimiento sea uniforme en todos los dispositivos.

   Ejemplo sin Time.deltaTime:
   float move = Input.GetAxis("Vertical") * moveSpeed;
   Resultado: El objeto se moverá de manera inconsistente y dependerá del rendimiento del hardware.

2. Comparación de rotación en FixedUpdate con el movimiento en Update:
   - Rotación en FixedUpdate:
     - FixedUpdate se ejecuta en intervalos fijos (normalmente 50 veces por segundo).
     - Es más predecible para cálculos relacionados con físicas, pero puede parecer menos fluido.
   - Movimiento en Update:
     - Update se ejecuta cada frame renderizado.
     - Es más adecuado para operaciones basadas en inputs del jugador o gráficos.
     - Utilizando Time.deltaTime, se logra un movimiento suave y consistente.

*/