using UnityEngine;
using UnityEngine.UI;

public class CarShopManager : MonoBehaviour
{
    public int currentCarIndex;
    public Text carNameText;
    public Button previousButton;
    public Button nextButton;
    public Button buyButton;

    private int[] carPrices = { 0, 2000, 5000, 10000, 25000, 50000 };
    private string[] carNames = { "Car 1", "Car 2", "Car 3", "Car 4", "Car 5" };
    private bool[] carsUnlocked = new bool[0];
    private ProfileData currentProfile;

    public void InitializeShop(ProfileData profile)
    {
        currentProfile = profile;

        // Inicializar `carsUnlocked` con el tamaño correcto
        carsUnlocked = new bool[carNames.Length];

        // Configura los coches desbloqueados basándose en el perfil activo
        for (int i = 0; i < currentProfile.ownedCars.Count; i++)
        {
            int ownedCarIndex = currentProfile.ownedCars[i];
            if (ownedCarIndex >= 0 && ownedCarIndex < carsUnlocked.Length)
            {
                carsUnlocked[ownedCarIndex] = true;
            }
        }
        
        UpdateCarUI();
    }

    private void UpdateCarUI()
    {
        if (currentCarIndex < 0 || currentCarIndex >= carsUnlocked.Length)
        {
            Debug.LogError("currentCarIndex is out of range!");
            return;
        }

        // Actualiza el texto del coche
        carNameText.text = $"{carNames[currentCarIndex]} - Price: {carPrices[currentCarIndex]}$";

        // Configura botones de navegación
        previousButton.interactable = currentCarIndex > 0;
        nextButton.interactable = currentCarIndex < carNames.Length - 1;

        // Configura el botón de compra
        bool isUnlocked = carsUnlocked[currentCarIndex];
        buyButton.interactable = !isUnlocked && currentProfile.money >= carPrices[currentCarIndex];
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

        if (currentProfile.money >= carPrice && !carsUnlocked[currentCarIndex])
        {
            currentProfile.money -= carPrice;
            carsUnlocked[currentCarIndex] = true;
            currentProfile.ownedCars.Add(currentCarIndex);

            Debug.Log($"{carNames[currentCarIndex]} bought successfully!");

            UpdateCarUI();
        }
        else
        {
            Debug.Log("Not enough money or car already purchased.");
        }
    }
}
