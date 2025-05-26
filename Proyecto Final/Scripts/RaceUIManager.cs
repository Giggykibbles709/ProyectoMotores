using UnityEngine;
using UnityEngine.UI;

public class RaceUIManager : MonoBehaviour
{
    public Racer playerRacer; // Referencia al corredor del jugador
    public Text lapCounterText;
    public Text positionText;
    public Text currentLapTimeText;
    public Text totalTimeText;

    private float currentLapTime = 0f;
    private float totalTime = 0f;

    private void Update()
    {
        if (playerRacer == null) return;

        // Actualizar tiempos
        currentLapTime += Time.deltaTime;
        totalTime += Time.deltaTime;

        // Actualizar textos
        lapCounterText.text = $"Lap: {playerRacer.currentLap + 1}/3";
        positionText.text = $"Position: {playerRacer.currentPosition}/{FindObjectsOfType<Racer>().Length}";
        currentLapTimeText.text = $"Lap Time: {FormatTime(currentLapTime)}";
        totalTimeText.text = $"Total Time: {FormatTime(totalTime)}";

        // Reset lap time al completar una vuelta
        if (playerRacer.lastCheckpointIndex == 0)
        {
            currentLapTime = 0f;
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);

        return $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }
}
