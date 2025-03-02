using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad de movimiento del personaje
    public float rotationSpeed = 10f; // Velocidad de rotación del personaje

    private float playerRadius = 0.5f; // Radio del jugador para la detección de colisiones
    private float playerHeight = 2f; // Altura del jugador para la detección de colisiones

    void Update()
    {
        // Obtener la entrada de movimiento (WASD o flechas)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");
        Vector3 inputVector = new Vector3(moveX, 0f, moveZ).normalized;

        // Dirección de movimiento
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.z);

        // Calcular la distancia de movimiento
        float moveDistance = moveSpeed * Time.deltaTime;

        // Verificar si el jugador puede moverse en la dirección deseada
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // Intentar moverse solo en la dirección X
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                // Moverse solo en X
                moveDir = moveDirX;
            }
            else
            {
                // Intentar moverse solo en la dirección Z
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    // Moverse solo en Z
                    moveDir = moveDirZ;
                }
                else
                {
                    // No se puede mover en ninguna dirección
                    moveDir = Vector3.zero;
                }
            }
        }

        // Aplicar movimiento si es posible
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        // Rotar hacia la dirección del movimiento
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
