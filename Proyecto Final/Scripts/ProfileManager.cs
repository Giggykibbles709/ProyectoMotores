using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ProfileData
{
    public string playerName;
    public string avatar;
    public string country;
}

public class ProfileManager : MonoBehaviour
{
    public Button[] profileButtons; // Botones de selección de perfiles
    public GameObject creationPanel; // Panel para crear el perfil
    public InputField nameInput; // Input para el nombre
    public Dropdown countryDropdown; // Dropdown para el país
    public Image avatarImage; // Imagen del avatar seleccionado
    public Sprite[] avatars; // Lista de sprites de avatares

    private int selectedProfileIndex = -1; // Perfil actualmente seleccionado
    private const string ProfileKey = "Profile"; // Prefijo para guardar los perfiles

    public GameObject mainmenuPanel; // Panel del menú principal

    private void Start()
    {
        LoadProfiles();
    }

    private void LoadProfiles()
    {
        // Cargar y mostrar los datos de los perfiles en los botones
        for (int i = 0; i < profileButtons.Length; i++)
        {
            int index = i; // Necesario para evitar problemas con closures
            string profileKey = $"{ProfileKey}{index}";

            if (PlayerPrefs.HasKey(profileKey))
            {
                ProfileData profile = JsonUtility.FromJson<ProfileData>(PlayerPrefs.GetString(profileKey));
                profileButtons[i].GetComponentInChildren<Text>().text = profile.playerName; // Muestra el nombre en el botón
            }
            else
            {
                profileButtons[i].GetComponentInChildren<Text>().text = "Empty Profile";
            }

            // Configurar el botón para seleccionar un perfil
            profileButtons[i].onClick.AddListener(() => OnSelectProfile(index));
        }
    }

    public void OnSelectProfile(int index)
    {
        selectedProfileIndex = index;
        string profileKey = $"{ProfileKey}{index}";

        if (PlayerPrefs.HasKey(profileKey))
        {
            Debug.Log($"Load profile {index}");
            mainmenuPanel.SetActive(true);
            // Cargar y mostrar detalles del perfil existente si lo deseas
        }
        else
        {
            OpenCreationPanel();
        }
    }

    private void OpenCreationPanel()
    {
        creationPanel.SetActive(true);
        nameInput.text = "";
        countryDropdown.value = 0;
        avatarImage.sprite = avatars[0]; // Avatar por defecto
    }

    public void OnSelectAvatar(int avatarIndex)
    {
        avatarImage.sprite = avatars[avatarIndex];
    }

    public void OnSaveProfile()
    {
        if (selectedProfileIndex < 0) return;

        ProfileData newProfile = new ProfileData
        {
            playerName = nameInput.text,
            avatar = avatarImage.sprite.name,
            country = countryDropdown.options[countryDropdown.value].text
        };

        string profileKey = $"{ProfileKey}{selectedProfileIndex}";
        PlayerPrefs.SetString(profileKey, JsonUtility.ToJson(newProfile));
        PlayerPrefs.Save();

        profileButtons[selectedProfileIndex].GetComponentInChildren<Text>().text = newProfile.playerName;
        creationPanel.SetActive(false);
        mainmenuPanel.SetActive(true);
    }
}
