using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    private int carSelected;
    private int circuitSelected;
    public int buyCarIndex;
    public int money;
    public Text carName;
    public GameObject previousButton;
    public GameObject nextButton;
    public GameObject buyButton;

    private int[] carPrices = { 0, 2000, 5000, 10000, 25000, 50000 }; // Precios de los coches
    private string[] carNames = { "Car 1", "Car 2", "Car 3", "Car 4", "Car 5" }; // Nombres de los coches
    private bool[] cochesDesbloqueados = { true, false, false, false, false }; // Estado de desbloqueo de los coches

    // Start is called before the first frame update
    void Start()
    {
        UpdateCarUI();
    }

    void UpdateCarUI()
    {
        // Actualiza el nombre y precio del coche
        carName.text = $"{carNames[buyCarIndex]} - Price: {carPrices[buyCarIndex]}$";

        // Configura los botones de navegación
        previousButton.SetActive(buyCarIndex > 0);
        nextButton.SetActive(buyCarIndex < carNames.Length - 1);

        // Configura el botón de compra
        buyButton.SetActive(!cochesDesbloqueados[buyCarIndex] && carPrices[buyCarIndex] > 0);
    }

    public void SelectCar(int carIndex)
    {
        carSelected = carIndex;
        Debug.Log("Car selected: " + carSelected);
    }

    public void SelectCircuit(int circuitIndex)
    {
        circuitSelected = circuitIndex;
        Debug.Log("Circuit selected: " + circuitSelected);
    }

    public void StartGame()
    {
        // Carga la escena basada en el circuito seleccionado
        SceneManager.LoadScene(circuitSelected + 1);
    }

    public void NextCar()
    {
        if (buyCarIndex < carNames.Length - 1)
        {
            buyCarIndex++;
            UpdateCarUI();
        }
    }

    public void PreviousCar()
    {
        if (buyCarIndex > 0)
        {
            buyCarIndex--;
            UpdateCarUI();
        }
    }

    public void BuyCar()
    {
        int carPrice = carPrices[buyCarIndex];
        if (money >= carPrice && !cochesDesbloqueados[buyCarIndex])
        {
            money -= carPrice;
            cochesDesbloqueados[buyCarIndex] = true;
            Debug.Log($"{carNames[buyCarIndex]} bought successfully!");
            UpdateCarUI();
        }
        else
        {
            Debug.Log("Not enough money or car already purchased.");
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
