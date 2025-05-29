using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RaceMenuManager : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject sceneCamera;
    public GameObject playerCamera;

    [Header("UI")]
    public GameObject playerHUD;
    public GameObject startPanel;
    public GameObject pausePanel;
    public Text countdownText;
    public GameObject finishMenuPanel;
    public Sprite[] countryFlags;
    public Image profileCountryFlag;
    public Text profileNameText;
    public Image[] standingsFlags;
    public Text[] standingsNames;
    public Text rewardText;

    [Header("Cars")]
    public GameObject playerCar;
    public GameObject[] aiCars;

    private bool raceWon;
    private bool canPause;
    private int playerPosition = 0;

    private PlayerCarController playerController;
    private AICarController[] aiControllers;

    void Start()
    {
        Time.timeScale = 0f; // Pausa el juego al inicio
        canPause = false;

        int countryIndex = PlayerPrefs.GetInt("CountryIndex", 0);
        profileCountryFlag.sprite = (countryIndex >= 0 && countryIndex < countryFlags.Length) ? countryFlags[countryIndex] : null;
        profileNameText.text = "4. " + PlayerPrefs.GetString("PlayerName", "Player");

        if (playerCar == null)
        {
            playerCar = GameObject.FindWithTag("Player");
            if (playerCar == null)
                Debug.LogError("No se encontró un coche del jugador con el tag 'Player'.");
        }

        if (playerCar != null)
        {
            playerController = playerCar.GetComponent<PlayerCarController>();
            if (playerController == null)
                Debug.LogError("El objeto playerCar no tiene un componente PlayerCarController.");
        }

        aiControllers = new AICarController[aiCars.Length];
        for (int i = 0; i < aiCars.Length; i++)
        {
            if (aiCars[i] != null)
                aiControllers[i] = aiCars[i].GetComponent<AICarController>();
            else
                Debug.LogWarning($"AI car at index {i} is null.");
        }

        // Deshabilitar control de coches al inicio
        DisableCarControl(playerCar);
        foreach (var aiCar in aiCars)
            DisableCarControl(aiCar);
    }

    private void Update()
    {
        if (!canPause) return;

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (pausePanel.activeSelf)
                ContinueGame();
            else
                PauseGame();
        }
    }

    public void StartRace()
    {
        startPanel.SetActive(false);
        sceneCamera.SetActive(false);
        playerCamera.SetActive(true);
        countdownText.gameObject.SetActive(true);

        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        Time.timeScale = 1f;

        int countdown = 3;
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        countdownText.text = "¡GO!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);

        playerHUD.SetActive(true);
        RaceEventManager.TriggerCountdownFinished();

        EnableCarControl(playerCar);
        foreach (var aiCar in aiCars)
            EnableCarControl(aiCar);

        canPause = true;
    }

    private void DisableCarControl(GameObject car)
    {
        if (car == null) return;

        if (car.TryGetComponent(out PlayerCarController playerCtrl))
            playerCtrl.enabled = false;
        else if (car.TryGetComponent(out AICarController aiCtrl))
            aiCtrl.enabled = false;
    }

    private void EnableCarControl(GameObject car)
    {
        if (car == null) return;

        if (car.TryGetComponent(out PlayerCarController playerCtrl))
            playerCtrl.enabled = true;
        else if (car.TryGetComponent(out AICarController aiCtrl))
            aiCtrl.enabled = true;
    }

    public void ShowFinishMenu(Racer[] racers)
    {
        DisableCarControl(playerCar);
        foreach (var aiCar in aiCars)
            DisableCarControl(aiCar);

        canPause = false;
        sceneCamera.SetActive(true);
        playerCamera.SetActive(false);
        playerHUD.SetActive(false);

        if (finishMenuPanel == null) return;

        Array.Sort(racers, (a, b) => b.GetProgress().CompareTo(a.GetProgress()));

        for (int i = 0; i < racers.Length; i++)
        {
            if (i >= standingsFlags.Length || i >= standingsNames.Length) break;

            if (racers[i].isPlayer)
            {
                standingsNames[i].text = $"{i + 1}. {PlayerPrefs.GetString("PlayerName", "Player")}";
                int playerCountryIndex = PlayerPrefs.GetInt("CountryIndex", 0);
                standingsFlags[i].sprite = (playerCountryIndex >= 0 && playerCountryIndex < countryFlags.Length) ? countryFlags[playerCountryIndex] : null;
            }
            else
            {
                standingsNames[i].text = $"{i + 1}. {racers[i].racerName}";
                standingsFlags[i].sprite = (racers[i].countryIndex >= 0 && racers[i].countryIndex < countryFlags.Length) ? countryFlags[racers[i].countryIndex] : null;
            }
        }

        finishMenuPanel.SetActive(true);
        PlayerPrefs.Save();
    }

    public void ShowReward()
    {
        int selectedCircuit = PlayerPrefs.GetInt("SelectedCircuit", 0);

        int baseReward = playerPosition switch
        {
            0 => 1000,
            1 => 500,
            2 => 250,
            _ => 100,
        };

        int reward = selectedCircuit switch
        {
            1 => (int)(baseReward * 1.2f),
            2 => (int)(baseReward * 1.5f),
            3 => baseReward * 2,
            4 => baseReward * 3,
            _ => baseReward,
        };

        int currentMoney = PlayerPrefs.GetInt("Money", 0) + reward;
        PlayerPrefs.SetInt("Money", currentMoney);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        string sceneKey = $"SceneWon_{currentSceneIndex}";

        if (playerPosition == 0)
        {
            if (!PlayerPrefs.HasKey(sceneKey))
            {
                // Es la primera vez que se gana en esta escena, subir reputación
                int currentReputation = PlayerPrefs.GetInt("ReputationLevel", 1) + 1;
                PlayerPrefs.SetInt("ReputationLevel", currentReputation);
                PlayerPrefs.SetInt(sceneKey, 1); // Marcar la escena como ganada

                rewardText.text = $"You won ${reward} for this race and you leveled up your reputation unlocking a new circuit!";
            }
            else
            {
                // Ya se había ganado en esta escena antes
                rewardText.text = $"You won ${reward} for this race!";
            }
        }
        else
        {
            // La carrera no fue ganada
            rewardText.text = $"You won ${reward} for this race!";
        }

        PlayerPrefs.Save();
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        playerHUD.SetActive(false);
        pausePanel.SetActive(true);
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        playerHUD.SetActive(true);
    }

    public void RestartRace()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
