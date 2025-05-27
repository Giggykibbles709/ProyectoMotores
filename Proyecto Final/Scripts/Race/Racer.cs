using UnityEngine;

public class Racer : MonoBehaviour
{
    public Transform[] checkpoints; // Asignado desde el inspector
    public int currentLap = 0;
    public int lastCheckpointIndex = -1;
    public float distanceToNextCheckpoint = 0f;
    public int currentPosition = 0;
    public int maxLaps = 3; // Número máximo de vueltas
    public RaceMenuManager raceMenuManager; // Referencia al gestor del menú

    private bool[] visitedCheckpoints; // Rastrea los checkpoints visitados en esta vuelta
    private bool raceFinished = false; // Indica si la carrera ha terminado

    private void Start()
    {
        // Inicializa el estado de los checkpoints
        if (checkpoints != null && checkpoints.Length > 0)
        {
            visitedCheckpoints = new bool[checkpoints.Length];
        }
    }

    public float GetProgress()
    {
        if (lastCheckpointIndex < 0 || checkpoints.Length == 0) return 0f;

        // Progreso del checkpoint actual
        float checkpointProgress = (float)lastCheckpointIndex / checkpoints.Length;

        // Distancia al siguiente checkpoint
        Transform nextCheckpoint = checkpoints[(lastCheckpointIndex + 1) % checkpoints.Length];
        float segmentDistance = Vector3.Distance(
            checkpoints[lastCheckpointIndex].position,
            nextCheckpoint.position
        );

        float distanceProgress = segmentDistance > 0
            ? 1f - (distanceToNextCheckpoint / segmentDistance)
            : 0f;

        return currentLap + checkpointProgress + distanceProgress;
    }

    private void Update()
    {
        if (raceFinished) return; // No actualizar si la carrera terminó

        if (checkpoints == null || checkpoints.Length == 0) return;

        Transform nextCheckpoint = checkpoints[(lastCheckpointIndex + 1) % checkpoints.Length];
        distanceToNextCheckpoint = Vector3.Distance(transform.position, nextCheckpoint.position);

        // Actualiza el checkpoint actual como visitado
        if (lastCheckpointIndex >= 0 && lastCheckpointIndex < visitedCheckpoints.Length)
        {
            visitedCheckpoints[lastCheckpointIndex] = true;
        }
    }

    public void CheckpointReached(int checkpointIndex)
    {
        if (raceFinished) return;

        // Verifica si el checkpoint alcanzado es el siguiente en el orden
        if (lastCheckpointIndex == checkpointIndex - 1 ||
            (lastCheckpointIndex == checkpoints.Length - 1 && checkpointIndex == 0))
        {
            lastCheckpointIndex = checkpointIndex;

            // Si se alcanza el checkpoint 0, verifica si se completó una vuelta válida
            if (checkpointIndex == 0 && HasCompletedLap())
            {
                CompleteLap();
            }
        }
    }

    public bool HasCompletedLap()
    {
        // Verifica si todos los checkpoints han sido visitados
        foreach (bool visited in visitedCheckpoints)
        {
            if (!visited) return false;
        }

        // Reinicia el estado de los checkpoints para la siguiente vuelta
        ResetVisitedCheckpoints();
        return true;
    }

    private void ResetVisitedCheckpoints()
    {
        for (int i = 0; i < visitedCheckpoints.Length; i++)
        {
            visitedCheckpoints[i] = false;
        }
    }

    public void CompleteLap()
    {
        currentLap++;
        Debug.Log($"Lap Completed! Current Lap: {currentLap}");

        if (currentLap >= maxLaps)
        {
            FinishRace();
        }
    }

    private void FinishRace()
    {
        raceFinished = true;
        Debug.Log("Race Finished!");

        // Mostrar menú de fin de carrera
        if (raceMenuManager != null)
        {
            raceMenuManager.ShowFinishMenu(FindObjectsOfType<Racer>());
        }
    }
}
