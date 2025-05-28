using UnityEngine;
using System.Collections.Generic;

public class RaceManager : MonoBehaviour
{
    public List<Racer> racers = new List<Racer>();
    public GameObject[] carPrefabs; // Prefabs de los coches que se pueden seleccionar
    public GameObject player;

    private void Start()
    {
        // Encuentra todos los corredores en la escena y los añade a la lista
        racers.AddRange(FindObjectsOfType<Racer>());

        int selectedCar = PlayerPrefs.GetInt("SelectedCar", 0); // Obtiene el coche seleccionado del PlayerPrefs
        GameObject playerCar = Instantiate(carPrefabs[selectedCar], player.transform.position, player.transform.rotation, player.transform); // Instancia el coche seleccionado

        // Asegúrate de que el objeto instanciado tenga un componente Racer
        Racer playerRacer = playerCar.GetComponent<Racer>();
        if (playerRacer != null)
        {
            racers.Add(playerRacer); // Agrega el corredor del jugador a la lista
        }
        else
        {
            Debug.LogError("El prefab del coche del jugador no tiene un componente Racer.");
        }
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
