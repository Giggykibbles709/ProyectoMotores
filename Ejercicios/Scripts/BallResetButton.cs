using UnityEngine;

public class BallResetButton : MonoBehaviour
{
    // Referencia al controlador del bal�n que ser� reseteado
    public BallController ball;

    // M�todo que se ejecuta al hacer clic en el bot�n de reset
    public void OnResetButtonClick()
    {
        // Verifica si la referencia al BallController est� configurada
        if (ball != null)
        {
            // Llama al m�todo para resetear el bal�n
            ball.ResetBall();
        }
    }
}
