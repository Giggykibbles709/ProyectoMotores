using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int checkpointIndex; // Índice del checkpoint en el circuito

    private void OnTriggerEnter(Collider other)
    {
        Racer racer = other.GetComponent<Racer>();
        if (racer != null)
        {
            if (racer.lastCheckpointIndex == checkpointIndex - 1 || (racer.lastCheckpointIndex == racer.checkpoints.Length - 1 && checkpointIndex == 0))
            {
                racer.lastCheckpointIndex = checkpointIndex;

                // Incrementa la vuelta si pasa por el checkpoint 0
                if (checkpointIndex == 0)
                {
                    racer.currentLap++;
                }
            }
        }
    }
}
