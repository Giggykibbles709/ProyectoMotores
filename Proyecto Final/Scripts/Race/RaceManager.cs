using UnityEngine;
using System.Collections.Generic;

public class RaceManager : MonoBehaviour
{
    public List<Racer> racers = new List<Racer>();

    private void Start()
    {
        // Encuentra todos los corredores en la escena y los añade a la lista
        racers.AddRange(FindObjectsOfType<Racer>());
    }

    private void Update()
    {
        UpdatePositions();
    }

    private void UpdatePositions()
    {
        // Ordenar la lista de corredores según su progreso
        racers.Sort((r1, r2) => r2.GetProgress().CompareTo(r1.GetProgress()));

        // Actualizar la posición de cada corredor
        for (int i = 0; i < racers.Count; i++)
        {
            racers[i].currentPosition = i + 1;
        }
    }
}
