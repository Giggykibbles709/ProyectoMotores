using UnityEngine;
using System.Linq;

public class RacePositionManager : MonoBehaviour
{
    public Racer[] racers; // Todos los corredores (incluyendo el jugador)

    private void Update()
    {
        if (racers == null || racers.Length == 0) return;

        // Ordenar corredores por progreso
        racers = racers.OrderByDescending(r => r.GetProgress()).ToArray();

        // Asignar posici�n a cada corredor
        for (int i = 0; i < racers.Length; i++)
        {
            racers[i].currentPosition = i + 1;
        }
    }
}
