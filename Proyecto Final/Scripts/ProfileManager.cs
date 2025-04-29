using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ProfileData
{
    public string playerName;
    public string avatar;
    public int countryIndex; // Índice del país seleccionado en el Dropdown
}

public class ProfileManager : MonoBehaviour
{
    public Button[] profileButtons; // Botones de selección de perfiles
    public Button[] deleteButtons; // Botones para borrar perfiles
    public GameObject creationPanel; // Panel para crear el perfil
    public InputField nameInput; // Input para el nombre
    public Dropdown countryDropdown; // Dropdown para el país
    public Image avatarImage; // Imagen del avatar seleccionado
    public Sprite[] avatars; // Lista de sprites de avatares
    public Sprite[] countryFlags; // Lista de sprites de banderas

    private int selectedProfileIndex = -1; // Perfil actualmente seleccionado
    private const string ProfileKey = "Profile"; // Prefijo para guardar los perfiles

    public GameObject mainmenuPanel; // Panel del menú principal
    public GameObject profileInfoPanel; // Panel para mostrar información del perfil
    public Text profileNameText; // Texto para mostrar el nombre
    public Image profileAvatarImage; // Imagen para mostrar el avatar
    public Image profileCountryFlagImage; // Imagen para mostrar la bandera del país

    private void Start()
    {
        ConfigureDropdown();
        LoadProfiles();
    }

    private void ConfigureDropdown()
    {
        // Limpiar las opciones actuales del Dropdown
        countryDropdown.options.Clear();

        // Añadir las banderas como opciones
        foreach (Sprite flag in countryFlags)
        {
            Dropdown.OptionData option = new Dropdown.OptionData
            {
                image = flag // Usar la bandera como imagen
            };
            countryDropdown.options.Add(option);
        }

        // Refrescar el Dropdown para aplicar los cambios
        countryDropdown.RefreshShownValue();
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
                deleteButtons[i].gameObject.SetActive(true); // Activar el botón de borrar
            }
            else
            {
                profileButtons[i].GetComponentInChildren<Text>().text = "Empty Profile";
                deleteButtons[i].gameObject.SetActive(false); // Desactivar el botón de borrar
            }

            // Configurar el botón para seleccionar un perfil
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
            Debug.Log($"Load profile {index}");
            mainmenuPanel.SetActive(true);
            LoadProfileInfo(index); // Cargar detalles del perfil
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
            countryIndex = countryDropdown.value // Guardar el índice seleccionado
        };

        string profileKey = $"{ProfileKey}{selectedProfileIndex}";
        PlayerPrefs.SetString(profileKey, JsonUtility.ToJson(newProfile));
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
            deleteButtons[index].gameObject.SetActive(false); // Desactivar el botón de borrar
        }

        Debug.Log($"Profile {index} deleted");
    }

    private void LoadProfileInfo(int index)
    {
        string profileKey = $"{ProfileKey}{index}";

        if (PlayerPrefs.HasKey(profileKey))
        {
            ProfileData profile = JsonUtility.FromJson<ProfileData>(PlayerPrefs.GetString(profileKey));

            // Mostrar datos en el panel de información
            profileNameText.text = $"{profile.playerName}";

            // Buscar el sprite correspondiente al nombre almacenado
            foreach (Sprite avatar in avatars)
            {
                if (avatar.name == profile.avatar)
                {
                    profileAvatarImage.sprite = avatar;
                    break;
                }
            }

            if (profile.countryIndex >= 0 && profile.countryIndex < countryFlags.Length)
            {
                profileCountryFlagImage.sprite = countryFlags[profile.countryIndex];
            }
        }
    }

    public void ShowProfileInfoPanel()
    {
        if (selectedProfileIndex < 0) return;

        profileInfoPanel.SetActive(true);
    }

    public void CloseProfileInfoPanel()
    {
        profileInfoPanel.SetActive(false);
    }
}
