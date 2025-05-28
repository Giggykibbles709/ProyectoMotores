using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ProfileData
{
    public string playerName;
    public string avatar;
    public int countryIndex;
    public int money;
    public int reputationLevel;

    public List<int> ownedCars;
    public List<bool> unlockedCircuits;
}

public class ProfileManager : MonoBehaviour
{
    public GameObject creationPanel;
    public GameObject mainMenuPanel;

    public InputField nameInput;
    public Dropdown countryDropdown;
    public Image avatarImage;
    public Sprite[] avatars;
    public Sprite[] countryFlags;

    public Text profileNameText;
    public Image profileCountryFlag;
    public Image profileAvatarImage;
    public Text profileMoneyText;
    public Text profileReputationText;

    private void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            LoadProfile();
            UpdateProfileDisplay();
            mainMenuPanel.SetActive(true);
        }
        else
        {
            OpenCreationPanel();
            ConfigureDropdown();
        }
    }

    private void OpenCreationPanel()
    {
        creationPanel.SetActive(true);
        nameInput.text = "";
        countryDropdown.value = 0;
        avatarImage.sprite = avatars[0];
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

    public void OnSelectAvatar(int avatarIndex)
    {
        avatarImage.sprite = avatars[avatarIndex];
    }

    public void SaveProfile()
    {
        ProfileData profile = new ProfileData
        {
            playerName = nameInput.text,
            avatar = avatarImage.sprite.name,
            countryIndex = countryDropdown.value,
            money = 0,
            reputationLevel = 1,
            ownedCars = new List<int> { 0 },
            unlockedCircuits = new List<bool> { true, false, false, false, false }
        };

        SaveProfileToPrefs(profile);

        creationPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
        UpdateProfileDisplay();
    }

    private void LoadProfile()
    {
        ProfileData profile = new ProfileData
        {
            playerName = PlayerPrefs.GetString("PlayerName"),
            avatar = PlayerPrefs.GetString("Avatar"),
            countryIndex = PlayerPrefs.GetInt("CountryIndex"),
            money = PlayerPrefs.GetInt("Money"),
            reputationLevel = PlayerPrefs.GetInt("ReputationLevel"),
            ownedCars = LoadList("OwnedCars"),
            unlockedCircuits = LoadBoolList("UnlockedCircuits")
        };

        UpdateProfileDisplay(profile);
    }

    public void UpdateProfileDisplay(ProfileData profile = null)
    {
        profile ??= LoadProfileFromPrefs();

        profileNameText.text = profile.playerName;
        profileCountryFlag.sprite = countryFlags[profile.countryIndex];
        profileAvatarImage.sprite = GetAvatarSprite(profile.avatar);
        profileMoneyText.text = $"Money: ${profile.money}";
        profileReputationText.text = $"Reputation Level: {profile.reputationLevel}";
    }

    private void SaveProfileToPrefs(ProfileData profile)
    {
        PlayerPrefs.SetString("PlayerName", profile.playerName);
        PlayerPrefs.SetString("Avatar", profile.avatar);
        PlayerPrefs.SetInt("CountryIndex", profile.countryIndex);
        PlayerPrefs.SetInt("Money", profile.money);
        PlayerPrefs.SetInt("ReputationLevel", profile.reputationLevel);

        SaveList("OwnedCars", profile.ownedCars);
        SaveBoolList("UnlockedCircuits", profile.unlockedCircuits);

        PlayerPrefs.Save();
    }

    private ProfileData LoadProfileFromPrefs()
    {
        return new ProfileData
        {
            playerName = PlayerPrefs.GetString("PlayerName"),
            avatar = PlayerPrefs.GetString("Avatar"),
            countryIndex = PlayerPrefs.GetInt("CountryIndex"),
            money = PlayerPrefs.GetInt("Money"),
            reputationLevel = PlayerPrefs.GetInt("ReputationLevel"),
            ownedCars = LoadList("OwnedCars"),
            unlockedCircuits = LoadBoolList("UnlockedCircuits")
        };
    }

    private Sprite GetAvatarSprite(string avatarName)
    {
        foreach (Sprite avatar in avatars)
        {
            if (avatar.name == avatarName)
                return avatar;
        }
        return avatars[0];
    }

    private void SaveList(string key, List<int> list)
    {
        PlayerPrefs.SetString(key, string.Join(",", list));
    }

    private List<int> LoadList(string key)
    {
        string data = PlayerPrefs.GetString(key, "");
        if (string.IsNullOrEmpty(data)) return new List<int>();
        return new List<int>(Array.ConvertAll(data.Split(','), int.Parse));
    }

    private void SaveBoolList(string key, List<bool> list)
    {
        PlayerPrefs.SetString(key, string.Join(",", list.ConvertAll(b => b ? "1" : "0")));
    }

    private List<bool> LoadBoolList(string key)
    {
        string data = PlayerPrefs.GetString(key, "");
        if (string.IsNullOrEmpty(data)) return new List<bool>();
        return new List<bool>(Array.ConvertAll(data.Split(','), s => s == "1"));
    }
}
