using UnityEngine;

public class CubeGameLoop : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento
    public float rotateSpeed = 50f; // Velocidad de rotaci�n

    void Update()
    {
        // Movimiento hacia adelante y atr�s con las teclas W y S
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(0, 0, move);

        // Rotaci�n con las teclas A y D
        float rotate = Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;
        transform.Rotate(0, rotate, 0);
    }

    void FixedUpdate()
    {
        // Rotaci�n autom�tica constante en FixedUpdate
        transform.Rotate(Vector3.up, rotateSpeed * Time.fixedDeltaTime);
    }
}
