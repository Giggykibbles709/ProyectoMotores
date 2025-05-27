using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject carSelectionPanel;
    public GameObject circuitSelectionPanel;

    [Header("Car Buttons")]
    public GameObject[] carButtons; // Array de botones de coches

    [Header("Circuit Buttons")]
    public GameObject[] circuitButtons; // Array de botones de circuitos

    private ProfileData activeProfile;

    public void Play()
    {
        activeProfile = FindObjectOfType<ProfileManager>()?.activeProfile;

        if (activeProfile == null)
        {
            Debug.LogError("No active profile found. Please ensure a profile is loaded.");
            return;
        }

        // Inicia con el panel de selección de coches
        carSelectionPanel.SetActive(true);
        circuitSelectionPanel.SetActive(false);

        UpdateCarButtons();
        UpdateCircuitButtons();
    }

    private void UpdateCarButtons()
    {
        for (int i = 0; i < carButtons.Length; i++)
        {
            bool isCarUnlocked = activeProfile.ownedCars.Contains(i);
            carButtons[i].GetComponent<UnityEngine.UI.Button>().interactable = isCarUnlocked;
        }
    }

    private void UpdateCircuitButtons()
    {
        for (int i = 0; i < circuitButtons.Length; i++)
        {
            bool isCircuitUnlocked = activeProfile.unlockedCircuits[i];
            circuitButtons[i].GetComponent<UnityEngine.UI.Button>().interactable = isCircuitUnlocked;
        }
    }

    public void SelectCar(int carIndex)
    {
        if (activeProfile.ownedCars.Contains(carIndex))
        {
            PlayerPrefs.SetInt("SelectedCar", carIndex);
            carSelectionPanel.SetActive(false);
            circuitSelectionPanel.SetActive(true);
        }
    }

    public void SelectCircuit(int circuitIndex)
    {
        if (activeProfile.unlockedCircuits[circuitIndex])
        {
            PlayerPrefs.SetInt("SelectedCircuit", circuitIndex);
            LoadCircuitScene(circuitIndex);
        }
    }

    private void LoadCircuitScene(int circuitIndex)
    {
        string circuitSceneName = "Circuit" + (circuitIndex + 1); // Asumiendo nombres como "Circuit1", "Circuit2", etc.
        SceneManager.LoadScene(circuitSceneName);
    }
}
