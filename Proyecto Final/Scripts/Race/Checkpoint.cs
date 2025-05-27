using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex; // Índice del checkpoint en el circuito

    private void OnTriggerEnter(Collider other)
    {
        Racer racer = other.GetComponent<Racer>();
        if (racer != null)
        {
            // Verifica si el checkpoint es el siguiente en el orden
            if (racer.lastCheckpointIndex == checkpointIndex - 1 ||
                (racer.lastCheckpointIndex == racer.checkpoints.Length - 1 && checkpointIndex == 0))
            {
                racer.lastCheckpointIndex = checkpointIndex;

                // Si pasa por el checkpoint 0, verifica si completó todos los checkpoints
                if (checkpointIndex == 0 && racer.HasCompletedLap())
                {
                    racer.CompleteLap();
                }
            }
        }
    }
}
