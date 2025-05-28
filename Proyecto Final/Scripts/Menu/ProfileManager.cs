using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ProfileData
{
    public string playerName;
    public string avatar;
    public int countryIndex;
    public int money = 0;
    public int racesPlayed = 0;
    public int racesWon = 0;
    public int reputationLevel = 0;

    public List<int> ownedCars = new List<int>(); // Coches comprados
    public List<bool> unlockedCircuits = new List<bool>(); // Circuitos desbloqueados
}

public class ProfileManager : MonoBehaviour
{
    public Button[] profileButtons;
    public Button[] deleteButtons;
    public GameObject creationPanel;
    public GameObject mainMenuPanel;
    public GameObject profilesPanel;
    public InputField nameInput;
    public Dropdown countryDropdown;
    public Image avatarImage;
    public Sprite[] avatars;
    public Sprite[] countryFlags;

    public GameObject profileInfoPanel;
    public Text profileNameText;
    public Image profileCountryFlag;
    public Image profileAvatarImage;
    public Text profileMoneyText;
    public Text profileRacesPlayedText;
    public Text profileRacesWonText;
    public Text profileReputationText;

    private int selectedProfileIndex = -1;
    private const string ProfileKey = "Profile";

    public ProfileData activeProfile;
    public CarShopManager carShopManager;

    private void Start()
    {
        // Verificar si hay un perfil seleccionado
        if (PlayerPrefs.HasKey("SelectedProfileIndex"))
        {
            int savedProfileIndex = PlayerPrefs.GetInt("SelectedProfileIndex");

            // Cargar automáticamente el perfil si existe
            if (PlayerPrefs.HasKey($"{ProfileKey}{savedProfileIndex}"))
            {
                activeProfile = JsonUtility.FromJson<ProfileData>(PlayerPrefs.GetString($"{ProfileKey}{savedProfileIndex}"));
                selectedProfileIndex = savedProfileIndex;

                // Cargar nombre y país desde PlayerPrefs
                PlayerPrefs.SetString("PlayerName", activeProfile.playerName);
                PlayerPrefs.SetInt("CountryIndex", activeProfile.countryIndex);
                PlayerPrefs.SetInt("Money", activeProfile.money);
                PlayerPrefs.SetInt("RacesPlayed", activeProfile.racesPlayed);
                PlayerPrefs.SetInt("RacesWon", activeProfile.racesWon);
                PlayerPrefs.SetInt("ReputationLevel", activeProfile.reputationLevel);
                PlayerPrefs.Save();

                // Mostrar el menú principal directamente
                mainMenuPanel.SetActive(true);
                profilesPanel.SetActive(false);

                if (activeProfile != null)
                {
                    carShopManager.InitializeShop(activeProfile);
                }
                else
                {
                    Debug.LogError("Profile data is missing!");
                }
            }
            else
            {
                // Si el índice guardado no es válido, muestra el panel de creación/selección
                ConfigureDropdown();
                LoadProfiles();
            }
        }
        else
        {
            // Si no hay perfil seleccionado, muestra el panel de creación/selección
            ConfigureDropdown();
            LoadProfiles();
        }
    }

    private void ConfigureDropdown()
    {
        countryDropdown.options.Clear();

        foreach (Sprite flag in countryFlags)
        {
            Dropdown.OptionData option = new Dropdown.OptionData { image = flag };
            countryDropdown.options.Add(option);
        }

        countryDropdown.RefreshShownValue();
    }

    private void LoadProfiles()
    {
        for (int i = 0; i < profileButtons.Length; i++)
        {
            int index = i;
            string profileKey = $"{ProfileKey}{index}";

            if (PlayerPrefs.HasKey(profileKey))
            {
                ProfileData profile = JsonUtility.FromJson<ProfileData>(PlayerPrefs.GetString(profileKey));
                profileButtons[i].GetComponentInChildren<Text>().text = profile.playerName;
                deleteButtons[i].gameObject.SetActive(true);
            }
            else
            {
                profileButtons[i].GetComponentInChildren<Text>().text = "Empty Profile";
                deleteButtons[i].gameObject.SetActive(false);
            }

            profileButtons[i].onClick.AddListener(() => OnSelectProfile(index));
            deleteButtons[i].onClick.AddListener(() => DeleteProfile(index));
        }
    }

    public void OnSelectProfile(int index)
    {
        selectedProfileIndex = index;
        string profileKey = $"{ProfileKey}{index}";

        if (PlayerPrefs.HasKey(profileKey))
        {
            // Cargar el perfil seleccionado
            activeProfile = JsonUtility.FromJson<ProfileData>(PlayerPrefs.GetString(profileKey));

            // Guardar el índice del perfil seleccionado
            PlayerPrefs.SetInt("SelectedProfileIndex", selectedProfileIndex);
            PlayerPrefs.SetString("PlayerName", activeProfile.playerName);
            PlayerPrefs.SetInt("CountryIndex", activeProfile.countryIndex);
            PlayerPrefs.SetInt("Money", activeProfile.money);
            PlayerPrefs.SetInt("RacesPlayed", activeProfile.racesPlayed);
            PlayerPrefs.SetInt("RacesWon", activeProfile.racesWon);
            PlayerPrefs.SetInt("ReputationLevel", activeProfile.reputationLevel);
            PlayerPrefs.Save();

            mainMenuPanel.SetActive(true);
            profilesPanel.SetActive(false);

            if (activeProfile != null)
            {
                carShopManager.InitializeShop(activeProfile);
            }
            else
            {
                Debug.LogError("Profile data is missing!");
            }
        }
        else
        {
            // Abrir el panel de creación si no hay perfil
            OpenCreationPanel();
        }
    }

    public void OnSelectAvatar(int avatarIndex)
    {
        avatarImage.sprite = avatars[avatarIndex];
    }

    public void DisplayProfileInfo()
    {
        if (activeProfile == null) return;

        mainMenuPanel.SetActive(false);
        profileInfoPanel.SetActive(true);
        profileNameText.text = activeProfile.playerName;
        profileCountryFlag.sprite = countryFlags[activeProfile.countryIndex];
        profileAvatarImage.sprite = GetAvatarSprite(activeProfile.avatar);
        profileMoneyText.text = $"Money: ${activeProfile.money}";
        profileRacesPlayedText.text = $"Races Played: {activeProfile.racesPlayed}";
        profileRacesWonText.text = $"Races Won: {activeProfile.racesWon}";
        profileReputationText.text = $"Reputation Level: {activeProfile.reputationLevel}";
    }

    private Sprite GetAvatarSprite(string avatarName)
    {
        foreach (Sprite avatar in avatars)
        {
            if (avatar.name == avatarName)
                return avatar;
        }
        return avatars[0]; // Retorna un avatar por defecto si no coincide
    }

    private void OpenCreationPanel()
    {
        creationPanel.SetActive(true);
        nameInput.text = "";
        countryDropdown.value = 0;
        avatarImage.sprite = avatars[0];
    }

    public void OnSaveProfile()
    {
        if (selectedProfileIndex < 0) return;

        ProfileData newProfile = new ProfileData
        {
            playerName = nameInput.text,
            avatar = avatarImage.sprite.name,
            countryIndex = countryDropdown.value
        };

        // Por defecto desbloquea solo el primer circuito
        newProfile.unlockedCircuits.Add(true);
        for (int i = 1; i < 5; i++) // Supongamos 5 circuitos
        {
            newProfile.unlockedCircuits.Add(false);
        }

        // Desbloquea el primer coche
        if (!newProfile.ownedCars.Contains(0))
        {
            newProfile.ownedCars.Add(0); // El índice 0 representa el primer coche
        }

        string profileKey = $"{ProfileKey}{selectedProfileIndex}";
        PlayerPrefs.SetString(profileKey, JsonUtility.ToJson(newProfile));
        PlayerPrefs.SetString("PlayerName", newProfile.playerName);
        PlayerPrefs.SetInt("CountryIndex", newProfile.countryIndex);
        PlayerPrefs.SetInt("Money", newProfile.money);
        PlayerPrefs.SetInt("RacesPlayed", newProfile.racesPlayed);
        PlayerPrefs.SetInt("RacesWon", newProfile.racesWon);
        PlayerPrefs.SetInt("ReputationLevel", newProfile.reputationLevel);
        PlayerPrefs.Save();

        profileButtons[selectedProfileIndex].GetComponentInChildren<Text>().text = newProfile.playerName;
        deleteButtons[selectedProfileIndex].gameObject.SetActive(true);
        creationPanel.SetActive(false);
    }

    public void DeleteProfile(int index)
    {
        string profileKey = $"{ProfileKey}{index}";

        if (PlayerPrefs.HasKey(profileKey))
        {
            PlayerPrefs.DeleteKey(profileKey);
            PlayerPrefs.Save();

            profileButtons[index].GetComponentInChildren<Text>().text = "Empty Profile";
            deleteButtons[index].gameObject.SetActive(false);

            if (selectedProfileIndex == index)
            {
                profileInfoPanel.SetActive(false);
                selectedProfileIndex = -1;
                activeProfile = null;
            }
        }
    }

    public void CloseProfileInfoPanel()
    {
        profileInfoPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
