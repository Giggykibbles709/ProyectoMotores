using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject carSelectionPanel;
    public GameObject circuitSelectionPanel;

    [Header("Car Buttons")]
    public GameObject[] carButtons; // Array de botones de coches

    [Header("Circuit Buttons")]
    public GameObject[] circuitButtons; // Array de botones de circuitos

    [Header("Reputation Requirements")]
    public int[] reputationRequirements; // Niveles de reputación requeridos para cada circuito

    private List<int> ownedCars = new List<int>();
    private bool[] unlockedCircuits;

    private int totalCircuits;

    private void Start()
    {
        // Inicializa el sistema cuando arranque (puedes llamar a Play() aquí o desde otro script)
        Play();
    }

    public void Play()
    {
        carSelectionPanel.SetActive(true);
        circuitSelectionPanel.SetActive(false);

        LoadOwnedCars();
        LoadUnlockedCircuits();

        UpdateCarButtons();
        UpdateCircuitButtons();
    }

    private void LoadOwnedCars()
    {
        ownedCars.Clear();

        // Cargar coches desbloqueados desde PlayerPrefs, guardados como string
        string ownedCarsData = PlayerPrefs.GetString("OwnedCars", "0"); // Por defecto desbloquea el coche 0
        string[] ownedCarsIndices = ownedCarsData.Split(',');

        foreach (string s in ownedCarsIndices)
        {
            if (int.TryParse(s, out int carIndex))
            {
                ownedCars.Add(carIndex);
            }
        }
    }

    private void LoadUnlockedCircuits()
    {
        totalCircuits = circuitButtons.Length;
        unlockedCircuits = new bool[totalCircuits];

        int reputationLevel = PlayerPrefs.GetInt("ReputationLevel", 1);

        for (int i = 0; i < totalCircuits; i++)
        {
            int reputationRequired = i < reputationRequirements.Length ? reputationRequirements[i] : int.MaxValue;
            unlockedCircuits[i] = reputationLevel >= reputationRequired;

            // Guardar el estado de desbloqueo en PlayerPrefs
            PlayerPrefs.SetInt("CircuitUnlocked_" + i, unlockedCircuits[i] ? 1 : 0);
        }
    }

    private void UpdateCarButtons()
    {
        for (int i = 0; i < carButtons.Length; i++)
        {
            bool isCarUnlocked = ownedCars.Contains(i);
            carButtons[i].GetComponent<Button>().interactable = isCarUnlocked;
        }
    }

    private void UpdateCircuitButtons()
    {
        for (int i = 0; i < circuitButtons.Length; i++)
        {
            bool isCircuitUnlocked = unlockedCircuits[i];
            circuitButtons[i].GetComponent<Button>().interactable = isCircuitUnlocked;
        }
    }

    public void SelectCar(int carIndex)
    {
        PlayerPrefs.SetInt("SelectedCar", carIndex);
        carSelectionPanel.SetActive(false);
        circuitSelectionPanel.SetActive(true);
    }

    public void SelectCircuit(int circuitIndex)
    {
        PlayerPrefs.SetInt("SelectedCircuit", circuitIndex);
        LoadCircuitScene(circuitIndex);
    }

    private void LoadCircuitScene(int circuitIndex)
    {
        string circuitSceneName = "Circuit" + (circuitIndex + 1);
        SceneManager.LoadScene(circuitSceneName);
    }
}
