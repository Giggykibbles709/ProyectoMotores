using System.Collections.Generic;
using UnityEngine;

public class RacePositionManager : MonoBehaviour
{
    public List<Racer> racers = new List<Racer>();

    private void Update()
    {
        UpdatePositions();
    }

    private void UpdatePositions()
    {
        // Ordena los corredores por progreso descendente
        racers.Sort((a, b) => b.GetProgress().CompareTo(a.GetProgress()));

        // Asigna las posiciones actualizadas a cada corredor
        for (int i = 0; i < racers.Count; i++)
        {
            racers[i].currentPosition = i + 1;
        }
    }

    public void RegisterRacer(Racer racer)
    {
        if (!racers.Contains(racer))
        {
            racers.Add(racer);
        }
    }

    public void UnregisterRacer(Racer racer)
    {
        if (racers.Contains(racer))
        {
            racers.Remove(racer);
        }
    }
}
