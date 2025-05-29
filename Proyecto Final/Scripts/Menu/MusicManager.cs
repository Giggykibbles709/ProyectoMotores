using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public Slider musicSlider;
    public AudioSource musicSource;
    public AudioClip[] playlist;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private AudioClip PickRandomClip()
    {
        return playlist[Random.Range(0, playlist.Length)];
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
        InitializeMusicSettings();
        musicSource.Play();
    }

    void Update()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.clip = PickRandomClip();
            musicSource.Play();
        }
    }

    public void ChangeVolume()
    {
        musicSource.volume = musicSlider.value;
        SaveSoundSettings();
    }

    public void InitializeMusicSettings()
    {
        if (!PlayerPrefs.HasKey("MusicVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 0.5f);
        }

        LoadSoundSettings();
    }

    private void LoadSoundSettings()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
    }

    private void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
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
        musicSlider = GameObject.Find("Music Slider").GetComponent<Slider>();
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(delegate { ChangeVolume(); });
            LoadSoundSettings();
        }
    }
}
