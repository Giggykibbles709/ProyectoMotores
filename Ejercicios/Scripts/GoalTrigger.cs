using System.Collections;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public BallController ballController;
    public GameObject goalText; // Referencia al texto de gol en la UI
    public float resetDelay = 2f;
    public int scoringPlayer = 1; // el jugador que mete aquí suma punto

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            // Mostrar texto
            if (goalText != null)
                goalText.SetActive(true);

            ScoreManager.Instance.GoalScored(scoringPlayer);

            // Resetear el balón después de unos segundos
            StartCoroutine(ResetBallAfterDelay());
        }
    }

    private IEnumerator ResetBallAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);

        // Ocultar el mensaje de gol
        if (goalText != null)
            goalText.SetActive(false);

        // Resetear el balón
        ballController.ResetBall();
    }
}
