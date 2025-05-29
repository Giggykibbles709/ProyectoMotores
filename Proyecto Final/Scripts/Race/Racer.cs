using UnityEngine;

public class Racer : MonoBehaviour
{
    public Transform[] checkpoints;
    public int currentLap = 0;
    public int lastCheckpointIndex = -1;
    public float distanceToNextCheckpoint = 0f;
    public int currentPosition = 0;
    public int maxLaps = 3; // Número máximo de vueltas
    public RaceMenuManager raceMenuManager; // Referencia al gestor del menú

    public bool isPlayer;
    public string racerName;
    public int countryIndex;

    private bool[] visitedCheckpoints; // Rastrea los checkpoints visitados en esta vuelta
    public bool raceFinished = false; // Indica si la carrera ha terminado
    private bool raceStarted = false; // Indica si la carrera ha comenzado
    public float CurrentLapTime { get; private set; } = 0f; // Tiempo de la vuelta actual

    private void Start()
    {
        // Asigna automáticamente los checkpoints buscando un objeto padre con un tag específico
        GameObject checkpointsParent = GameObject.FindWithTag("Checkpoints");
        if (checkpointsParent != null)
        {
            checkpoints = new Transform[checkpointsParent.transform.childCount];
            for (int i = 0; i < checkpoints.Length; i++)
            {
                checkpoints[i] = checkpointsParent.transform.GetChild(i);
            }
        }
        else
        {
            Debug.LogError("No se encontraron los checkpoints en la escena. Asegúrate de que el objeto padre tenga el tag 'Checkpoints'.");
        }

        // Asigna automáticamente el RaceMenuManager buscando por tipo
        raceMenuManager = FindObjectOfType<RaceMenuManager>();
        if (raceMenuManager == null)
        {
            Debug.LogError("No se encontró un RaceMenuManager en la escena.");
        }

        // Inicializa el estado de los checkpoints
        if (checkpoints != null && checkpoints.Length > 0)
        {
            visitedCheckpoints = new bool[checkpoints.Length];
        }

        // Escuchar el evento de inicio de la carrera
        RaceEventManager.OnCountdownFinished += StartRace;
    }

    private void OnDestroy()
    {
        // Desuscribirse para evitar errores si el objeto es destruido
        RaceEventManager.OnCountdownFinished -= StartRace;
    }

    private void Update()
    {
        if (!raceStarted || raceFinished) return; // No actualizar si la carrera no comenzó o ya terminó
        if (checkpoints == null || checkpoints.Length == 0) return;

        // Actualiza el tiempo de la vuelta actual
        CurrentLapTime += Time.deltaTime;

        Transform nextCheckpoint = checkpoints[(lastCheckpointIndex + 1) % checkpoints.Length];
        distanceToNextCheckpoint = Vector3.Distance(transform.position, nextCheckpoint.position);

        // Marca el checkpoint actual como visitado
        if (lastCheckpointIndex >= 0 && lastCheckpointIndex < visitedCheckpoints.Length)
        {
            visitedCheckpoints[lastCheckpointIndex] = true;
        }
    }

    public float GetProgress()
    {
        if (lastCheckpointIndex < 0 || checkpoints.Length == 0) return 0f;

        // Progreso basado en los checkpoints visitados
        float checkpointProgress = (float)lastCheckpointIndex / checkpoints.Length;

        // Calcula el progreso dentro del segmento hacia el siguiente checkpoint
        Transform nextCheckpoint = checkpoints[(lastCheckpointIndex + 1) % checkpoints.Length];
        float segmentDistance = Vector3.Distance(
            checkpoints[lastCheckpointIndex].position,
            nextCheckpoint.position
        );

        float distanceProgress = segmentDistance > 0
            ? 1f - (Vector3.Distance(transform.position, nextCheckpoint.position) / segmentDistance)
            : 0f;

        // Retorna el progreso total como suma de la vuelta y los progresos
        return currentLap + checkpointProgress + distanceProgress;
    }

    public void CheckpointReached(int checkpointIndex)
    {
        if (!raceStarted || raceFinished) return;

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

        // Reinicia el tiempo de la vuelta
        CurrentLapTime = 0f;
    }

    private void StartRace()
    {
        raceStarted = true; // Inicia la carrera
        CurrentLapTime = 0f; // Reinicia el tiempo de la vuelta actual
    }
}
