using UnityEngine;
using UnityEngine.UI;

public class CarShopManager : MonoBehaviour
{
    public int currentCarIndex;
    public Text carNameText;
    public Button previousButton;
    public Button nextButton;
    public Button buyButton;

    private int[] carPrices = { 0, 2000, 5000, 10000, 25000 };
    private string[] carNames = { "Car 1", "Car 2", "Car 3", "Car 4", "Car 5" };
    private bool[] carsUnlocked;
    private int playerMoney;

    private void Start()
    {
        // Carga el dinero del jugador y los coches desbloqueados desde PlayerPrefs
        playerMoney = PlayerPrefs.GetInt("Money", 0);
        carsUnlocked = new bool[carNames.Length];
        LoadOwnedCars();

        // Inicializa la interfaz de la tienda
        UpdateCarUI();
    }

    private void LoadOwnedCars()
    {
        // Cargar la lista de coches desbloqueados desde PlayerPrefs
        string ownedCarsData = PlayerPrefs.GetString("OwnedCars", "0"); // Por defecto, solo el primer coche está desbloqueado
        string[] ownedCarsIndices = ownedCarsData.Split(',');

        foreach (string index in ownedCarsIndices)
        {
            if (int.TryParse(index, out int carIndex) && carIndex >= 0 && carIndex < carsUnlocked.Length)
            {
                carsUnlocked[carIndex] = true;
            }
        }
    }

    private void SaveOwnedCars()
    {
        // Guardar la lista de coches desbloqueados en PlayerPrefs
        string ownedCarsData = "";
        for (int i = 0; i < carsUnlocked.Length; i++)
        {
            if (carsUnlocked[i])
            {
                ownedCarsData += $"{i},";
            }
        }

        if (ownedCarsData.EndsWith(","))
        {
            ownedCarsData = ownedCarsData.TrimEnd(',');
        }

        PlayerPrefs.SetString("OwnedCars", ownedCarsData);
        PlayerPrefs.SetInt("Money", playerMoney);
        PlayerPrefs.Save();
    }

    private void UpdateCarUI()
    {
        if (currentCarIndex < 0 || currentCarIndex >= carsUnlocked.Length)
        {
            Debug.LogError("currentCarIndex is out of range!");
            return;
        }

        // Actualiza el texto del coche
        carNameText.text = carsUnlocked[currentCarIndex]
            ? $"{carNames[currentCarIndex]} - Owned"
            : $"{carNames[currentCarIndex]} - Price: {carPrices[currentCarIndex]}$";

        // Configura los botones de navegación
        previousButton.interactable = currentCarIndex > 0;
        nextButton.interactable = currentCarIndex < carNames.Length - 1;

        // Configura el botón de compra
        buyButton.interactable = !carsUnlocked[currentCarIndex] && playerMoney >= carPrices[currentCarIndex];
        buyButton.gameObject.SetActive(carPrices[currentCarIndex] > 0);
    }

    public void NextCar()
    {
        if (currentCarIndex < carNames.Length - 1)
        {
            currentCarIndex++;
            UpdateCarUI();
        }
    }

    public void PreviousCar()
    {
        if (currentCarIndex > 0)
        {
            currentCarIndex--;
            UpdateCarUI();
        }
    }

    public void BuyCar()
    {
        int carPrice = carPrices[currentCarIndex];

        if (playerMoney >= carPrice && !carsUnlocked[currentCarIndex])
        {
            // Deduce el precio del coche del dinero del jugador
            playerMoney -= carPrice;

            // Desbloquea el coche
            carsUnlocked[currentCarIndex] = true;

            // Guarda los cambios
            SaveOwnedCars();

            Debug.Log($"{carNames[currentCarIndex]} bought successfully!");

            // Actualiza la interfaz
            UpdateCarUI();
        }
        else
        {
            Debug.Log("Not enough money or car already purchased.");
        }
    }
}
