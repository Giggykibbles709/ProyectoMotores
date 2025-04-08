using System.Collections;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    // Referencia al controlador del bal�n para resetearlo
    public BallController ballController;

    // Referencia al texto de gol en la interfaz de usuario
    public GameObject goalText;

    // Tiempo de retraso antes de resetear el bal�n
    public float resetDelay = 2f;

    // Jugador que suma puntos al anotar en este gol
    public int scoringPlayer = 1;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra en el trigger es el bal�n
        if (other.CompareTag("Ball"))
        {
            // Mostrar el texto de gol en la UI si est� configurado
            if (goalText != null)
                goalText.SetActive(true);

            // Registrar el gol en el sistema de puntuaci�n
            ScoreManager.Instance.GoalScored(scoringPlayer);

            // Iniciar el reinicio del bal�n despu�s del retraso especificado
            StartCoroutine(ResetBallAfterDelay());
        }
    }

    private IEnumerator ResetBallAfterDelay()
    {
        // Esperar el tiempo especificado antes de continuar
        yield return new WaitForSeconds(resetDelay);

        // Ocultar el mensaje de gol de la UI si est� configurado
        if (goalText != null)
            goalText.SetActive(false);

        // Reiniciar la posici�n y estado del bal�n
        ballController.ResetBall();
    }
}
