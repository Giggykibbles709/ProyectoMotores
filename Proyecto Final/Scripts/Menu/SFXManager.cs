using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SFXManager : MonoBehaviour
{
    public Slider sfxSlider;
    public AudioSource sfxSource;
    public AudioClip hoverSound;
    public AudioClip clickSound;
    public AudioClip engineSound;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("SFX");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += LoadScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LoadScene;
    }

    void Start()
    {
        InitializeSFXSettings();
    }

    public void PlayHoverSound()
    {
        PlaySound(hoverSound);
    }

    public void PlayClickSound()
    {
        PlaySound(clickSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void ChangeVolume()
    {
        sfxSource.volume = sfxSlider.value;
        SaveSoundSettings();
    }

    public void InitializeSFXSettings()
    {
        if (!PlayerPrefs.HasKey("sfxVolume"))
        {
            PlayerPrefs.SetFloat("sfxVolume", 0.5f);
        }

        LoadSoundSettings();
    }

    private void LoadSoundSettings()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
    }

    private void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat("sfxVolume", sfxSlider.value);
    }

    private void LoadScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
        {
            ReassignSlider();
        }
    }

    private void ReassignSlider()
    {
        sfxSlider = GameObject.Find("SFX Slider").GetComponent<Slider>();
        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.AddListener(delegate { ChangeVolume(); });
            LoadSoundSettings();
        }
    }
}
