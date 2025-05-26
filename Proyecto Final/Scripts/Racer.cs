using UnityEngine;

public class Racer : MonoBehaviour
{
    public Transform[] checkpoints; // Asignado desde el inspector
    public int currentLap = 0;
    public int lastCheckpointIndex = -1;
    public float distanceToNextCheckpoint = 0f;
    public int currentPosition = 0;

    public float GetProgress()
    {
        if (lastCheckpointIndex < 0) return 0f;

        float checkpointProgress = lastCheckpointIndex / (float)checkpoints.Length;
        float distanceProgress = 1f - (distanceToNextCheckpoint / Vector3.Distance(
            checkpoints[lastCheckpointIndex].position,
            checkpoints[(lastCheckpointIndex + 1) % checkpoints.Length].position
        ));

        return currentLap + checkpointProgress + distanceProgress;
    }

    private void Update()
    {
        if (checkpoints == null || checkpoints.Length == 0) return;

        Transform nextCheckpoint = checkpoints[(lastCheckpointIndex + 1) % checkpoints.Length];
        distanceToNextCheckpoint = Vector3.Distance(transform.position, nextCheckpoint.position);
    }
}
