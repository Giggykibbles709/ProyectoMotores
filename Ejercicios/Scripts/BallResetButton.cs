using UnityEngine;

public class BallResetButton : MonoBehaviour
{
    // Referencia al controlador del balón que será reseteado
    public BallController ball;

    // Método que se ejecuta al hacer clic en el botón de reset
    public void OnResetButtonClick()
    {
        // Verifica si la referencia al BallController está configurada
        if (ball != null)
        {
            // Llama al método para resetear el balón
            ball.ResetBall();
        }
    }
}
