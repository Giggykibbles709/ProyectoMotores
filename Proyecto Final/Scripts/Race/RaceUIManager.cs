using UnityEngine;
using UnityEngine.UI;

public class RaceUIManager : MonoBehaviour
{
    public Racer playerRacer; // Referencia al corredor del jugador
    public Text lapCounterText;
    public Text positionText;
    public Text currentLapTimeText;
    public Text totalTimeText;

    private float totalTime = 0f;
    private bool raceStarted = false; // Indica si la carrera ha comenzado

    private void Start()
    {
        // Asignar automáticamente el corredor del jugador al inicio
        if (playerRacer == null)
        {
            playerRacer = FindObjectOfType<Racer>(); // Busca el objeto del jugador automáticamente
            if (playerRacer == null)
            {
                Debug.LogError("No se encontró un objeto Racer en la escena para el jugador.");
                return;
            }
        }

        // Escuchar un evento para cuando termine la cuenta atrás (puedes integrarlo según tu lógica existente)
        RaceEventManager.OnCountdownFinished += StartRace;
    }

    private void OnDestroy()
    {
        // Desuscribirse para evitar errores si el objeto es destruido
        RaceEventManager.OnCountdownFinished -= StartRace;
    }

    private void Update()
    {
        if (!raceStarted || playerRacer == null) return;

        // Actualizar tiempos
        totalTime += Time.deltaTime;

        // Actualizar textos
        lapCounterText.text = $"Lap: {playerRacer.currentLap + 1}/{playerRacer.maxLaps}";
        positionText.text = $"Position: {playerRacer.currentPosition}/{FindObjectsOfType<Racer>().Length}";
        currentLapTimeText.text = $"Lap Time: {FormatTime(playerRacer.CurrentLapTime)}";
        totalTimeText.text = $"Total Time: {FormatTime(totalTime)}";
    }

    private void StartRace()
    {
        raceStarted = true; // Indica que la carrera ha comenzado
        totalTime = 0f; // Reinicia el tiempo total
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000f) % 1000f);

        return $"{minutes:00}:{seconds:00}:{milliseconds:000}";
    }
}

// Clase auxiliar para manejar eventos de la carrera
public static class RaceEventManager
{
    public static System.Action OnCountdownFinished;

    public static void TriggerCountdownFinished()
    {
        OnCountdownFinished?.Invoke();
    }
}
