using UnityEngine;

public class CubeGameLoop : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    public float rotateSpeed = 50f; // Velocidad de rotaci�n
    private bool isManualRotation = false; // Bool para controlar la rotaci�n manual

    void Update()
    {
        // Movimiento hacia adelante y atr�s con las teclas W y S.
        // Usamos Time.deltaTime para garantizar que el movimiento sea consistente en todos los dispositivos,
        // independientemente de los FPS (frames por segundo) del sistema.
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(0, 0, move);

        // Rotaci�n con las teclas A y D.
        // Usamos Time.deltaTime para que la velocidad de rotaci�n sea consistente.
        float rotate = Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;
        //Usamos Mathf.Abs para devolver el valor absoluto del input del jugador.
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.01f) // Detectar si hay input en el eje horizontal
        {
            isManualRotation = true; // Activar bool de rotaci�n manual
            transform.Rotate(0, rotate, 0);
        }
        else
        {
            isManualRotation = false; // No hay input, volver a rotaci�n autom�tica
        }
    }

    void FixedUpdate()
    {
        // Rotaci�n autom�tica constante en FixedUpdate.
        // Aqu� usamos Time.fixedDeltaTime para garantizar que la rotaci�n sea consistente con los intervalos fijos de FixedUpdate.

        // Si no hay rotaci�n manual, continuar con la rotaci�n autom�tica.
        if (!isManualRotation)
        {
            transform.Rotate(Vector3.up, rotateSpeed * Time.fixedDeltaTime);
        }

        // FixedUpdate es �til para operaciones relacionadas con f�sicas, ya que se ejecuta a intervalos regulares,
        // independientemente de la velocidad de los frames.
    }
}

/*
Preguntas:

1. �Por qu� es necesario Time.deltaTime? �Qu� sucede si eliminas Time.deltaTime?
   - Time.deltaTime representa el tiempo que ha pasado desde el �ltimo cuadro. 
     Sin �l, las operaciones de movimiento y rotaci�n depender�an de los FPS, lo que 
     provocar�a que el objeto se mueva m�s r�pido en sistemas con m�s FPS y m�s lento en sistemas con menos FPS.
   - Usar Time.deltaTime asegura que el movimiento sea uniforme en todos los dispositivos.

   Ejemplo sin Time.deltaTime:
   float move = Input.GetAxis("Vertical") * moveSpeed;
   Resultado: El objeto se mover� de manera inconsistente y depender� del rendimiento del hardware.

2. Comparaci�n de rotaci�n en FixedUpdate con el movimiento en Update:
   - Rotaci�n en FixedUpdate:
     - FixedUpdate se ejecuta en intervalos fijos (normalmente 50 veces por segundo).
     - Es m�s predecible para c�lculos relacionados con f�sicas, pero puede parecer menos fluido.
   - Movimiento en Update:
     - Update se ejecuta cada frame renderizado.
     - Es m�s adecuado para operaciones basadas en inputs del jugador o gr�ficos.
     - Utilizando Time.deltaTime, se logra un movimiento suave y consistente.

*/