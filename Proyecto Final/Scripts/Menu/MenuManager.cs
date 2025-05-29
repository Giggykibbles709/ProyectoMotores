using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public GameObject settingsMenu;
    public Dropdown resolutionDropdown;

    // Start is called before the first frame update
    void Start()
    {
        settingsMenu.SetActive(false);

        resolutionDropdown.options.Clear();
        resolutionDropdown.options.Add(new Dropdown.OptionData("3840x2160"));
        resolutionDropdown.options.Add(new Dropdown.OptionData("2560x1440"));
        resolutionDropdown.options.Add(new Dropdown.OptionData("1920x1080"));
        resolutionDropdown.options.Add(new Dropdown.OptionData("1280x720"));

        resolutionDropdown.value = 2; // Default: 1080p
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
    }

    private void ChangeResolution(int index)
    {
        switch (index)
        {
            case 0:
                Screen.SetResolution(3840, 2160, Screen.fullScreen);
                break;
            case 1:
                Screen.SetResolution(2560, 1440, Screen.fullScreen);
                break;
            case 2:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
            case 3:
                Screen.SetResolution(1280, 720, Screen.fullScreen);
                break;
        }
    }

    public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SceneManager.LoadScene(0);
        Debug.Log("PlayerPrefs reset.");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
