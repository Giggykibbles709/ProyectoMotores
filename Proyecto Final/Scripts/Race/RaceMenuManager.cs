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
    public Text countdownText; // Texto para mostrar la cuenta regresiva
    public GameObject finishMenuPanel; // Panel de fin de carrera
    public Sprite[] countryFlags; // Sprites de banderas de países
    public Image profileCountryFlag;
    public Text profileNameText;
    public Image[] standingsFlags;
    public Text[] standingsNames;
    public Text rewardText; // Texto para mostrar la recompensa al finalizar la carrera

    [Header("Cars")]
    public GameObject playerCar; // Referencia al coche del jugador
    public GameObject[] aiCars; // Referencias a los coches de la IA

    public bool raceWon;
    public bool canPause; // Controla si se puede pausar el juego
    private int playerPosition = 0;
    private PlayerCarController playerController;
    private AICarController[] aiControllers;

    void Start()
    {
        Time.timeScale = 0f; // Pausa el juego al inicio
        canPause = false;

        profileCountryFlag.sprite = PlayerPrefs.GetInt("CountryIndex", 0) < countryFlags.Length ? countryFlags[PlayerPrefs.GetInt("CountryIndex", 0)] : null;
        profileNameText.text = "4. " + PlayerPrefs.GetString("PlayerName", "Player");

        // Asignación automática de playerCar buscando por tag
        if (playerCar == null)
        {
            playerCar = GameObject.FindWithTag("Player");
            if (playerCar == null)
            {
                Debug.LogError("No se encontró un coche del jugador en la escena con el tag 'Player'.");
            }
        }

        // Asignación del controlador del jugador
        if (playerCar != null)
        {
            playerController = playerCar.GetComponent<PlayerCarController>();
            if (playerController == null)
            {
                Debug.LogError("El objeto playerCar no tiene un componente PlayerCarController.");
            }
        }

        // Inicializa los controladores de IA
        aiControllers = new AICarController[aiCars.Length];
        for (int i = 0; i < aiCars.Length; i++)
        {
            aiControllers[i] = aiCars[i].GetComponent<AICarController>();
        }

        // Deshabilitar control de coches al inicio
        DisableCarControl(playerCar);
        foreach (var aiCar in aiCars)
        {
            DisableCarControl(aiCar);
        }
    }

    private void Update()
    {
        if (canPause)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                if (pausePanel.activeSelf)
                {
                    ContinueGame(); // Continúa el juego si el panel de pausa está activo
                }
                else
                {
                    PauseGame(); // Pausa el juego
                }
            }
        }
    }

    public void StartRace()
    {
        startPanel.SetActive(false); // Oculta el panel de inicio
        sceneCamera.SetActive(false); // Desactiva la cámara de la escena
        playerCamera.SetActive(true); // Activa la cámara del jugador
        countdownText.gameObject.SetActive(true); // Activa el texto de cuenta regresiva

        StartCoroutine(StartCountdown()); // Inicia la cuenta regresiva
    }

    private IEnumerator StartCountdown()
    {
        Time.timeScale = 1f; // Reactiva el juego

        int countdown = 3;
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString(); // Muestra el número actual
            yield return new WaitForSeconds(1f); // Espera un segundo
            countdown--;
        }

        countdownText.text = "¡GO!"; // Muestra el mensaje de inicio
        yield return new WaitForSeconds(1f); // Espera un segundo más
        countdownText.gameObject.SetActive(false); // Oculta el texto de cuenta regresiva

        playerHUD.SetActive(true); // Activa el HUD del jugador
        RaceEventManager.TriggerCountdownFinished(); // Llama al evento de finalización de la cuenta regresiva
        EnableCarControl(playerCar); // Habilita el control del coche del jugador
        canPause = true; // Permite pausar el juego
        foreach (var aiCar in aiCars)
        {
            EnableCarControl(aiCar); // Habilita el control de los coches de la IA
        }
    }

    private void DisableCarControl(GameObject car)
    {
        if (car.TryGetComponent(out PlayerCarController playerController))
        {
            playerController.enabled = false; // Desactiva el controlador del jugador
        }
        else if (car.TryGetComponent(out AICarController aiController))
        {
            aiController.enabled = false; // Desactiva el controlador de la IA
        }
    }

    private void EnableCarControl(GameObject car)
    {
        if (car.TryGetComponent(out PlayerCarController playerController))
        {
            playerController.enabled = true; // Activa el controlador del jugador
        }
        else if (car.TryGetComponent(out AICarController aiController))
        {
            aiController.enabled = true; // Activa el controlador de la IA
        }
    }

    public void ShowFinishMenu(Racer[] racers)
    {
        DisableCarControl(playerCar);
        foreach (var aiCar in aiCars)
        {
            DisableCarControl(aiCar);
        }

        canPause = false; // Desactiva la posibilidad de pausar el juego
        sceneCamera.SetActive(true); // Activa la cámara de la escena
        playerCamera.SetActive(false); // Desactiva la cámara del jugador
        playerHUD.SetActive(false); // Desactiva el HUD del jugador

        if (finishMenuPanel == null) return;

        // Ordenar los corredores por progreso
        System.Array.Sort(racers, (a, b) => b.GetProgress().CompareTo(a.GetProgress()));

        // Mostrar resultados en la UI
        for (int i = 0; i < racers.Length; i++)
        {
            if (i < standingsFlags.Length && i < standingsNames.Length)
            {
                if (racers[i].isPlayer) // Identificar si es el jugador
                {
                    // Asignar el nombre y la bandera desde PlayerPrefs
                    standingsNames[i].text = $"{i + 1}. {PlayerPrefs.GetString("PlayerName", "Player")}";
                    int countryIndex = PlayerPrefs.GetInt("CountryIndex", 0);
                    standingsFlags[i].sprite = (countryIndex >= 0 && countryIndex < countryFlags.Length) ? countryFlags[countryIndex] : null;
                    int racePlayed = PlayerPrefs.GetInt("RacesPlayed", 0);
                    racePlayed += 1;
                    PlayerPrefs.SetInt("RacesPlayed", racePlayed);

                    playerPosition = i;

                    // Si el jugador está en el primer lugar, incrementar las carreras ganadas
                    if (i == 0) // Primer lugar
                    {
                        int racesWon = PlayerPrefs.GetInt("RacesWon", 0);
                        PlayerPrefs.SetInt("RacesWon", racesWon + 1);
                        raceWon = true;
                    }
                }
                else
                {
                    // Asignar el nombre y la bandera desde el script Racer
                    standingsNames[i].text = $"{i + 1}. {racers[i].racerName}";
                    standingsFlags[i].sprite = (racers[i].countryIndex >= 0 && racers[i].countryIndex < countryFlags.Length) ? countryFlags[racers[i].countryIndex] : null;
                }
            }
        }

        // Activar el menú de fin de carrera
        finishMenuPanel.SetActive(true);

        // Guardar los cambios en PlayerPrefs
        PlayerPrefs.Save();
    }

    public void ShowReward()
    {
        // Obtener el circuito seleccionado de PlayerPrefs
        int selectedCircuit = PlayerPrefs.GetInt("SelectedCircuit", 0);

        // Definir recompensas base por posición
        int baseReward = 0;
        switch (playerPosition)
        {
            case 0:
                baseReward = 1000; // Primer lugar
                break;
            case 1:
                baseReward = 500; // Segundo lugar
                break;
            case 2:
                baseReward = 250; // Tercer lugar
                break;
            default:
                baseReward = 100; // Otros lugares
                break;
        }

        // Aplicar modificadores según el circuito seleccionado
        int reward = 0;
        switch (selectedCircuit)
        {
            case 0: // Circuito 1
                reward = baseReward; // Sin cambio
                break;
            case 1: // Circuito 2
                reward = (int)(baseReward * 1.2f); // 20% más
                break;
            case 2: // Circuito 3
                reward = (int)(baseReward * 1.5f); // 50% más
                break;
            case 3: // Circuito 4
                reward = baseReward * 2; // Doble recompensa
                break;
            case 4: // Circuito 5
                reward = baseReward * 3; // Triple recompensa
                break;
            default:
                reward = baseReward; // Por defecto, sin cambio
                break;
        }

        // Obtener el dinero actual de PlayerPrefs
        int currentMoney = PlayerPrefs.GetInt("Money", 0);
        
        // Sumar la recompensa al dinero actual
        currentMoney += reward;

        // Guardar el nuevo total en PlayerPrefs
        PlayerPrefs.SetInt("Money", currentMoney);

        // Verificar si este circuito ya ha sido ganado antes
        string wonCircuitsKey = "WonCircuits";
        string wonCircuits = PlayerPrefs.GetString(wonCircuitsKey, "");

        // Comprobar si el circuito ya está en la lista de circuitos ganados
        if (raceWon && !wonCircuits.Contains(selectedCircuit.ToString()))
        {
            // Incrementar la reputación del jugador
            int currentReputation = PlayerPrefs.GetInt("ReputationLevel", 0);
            currentReputation += 1;
            PlayerPrefs.SetInt("ReputationLevel", currentReputation);

            // Agregar el circuito a la lista de circuitos ganados
            wonCircuits += selectedCircuit.ToString() + ",";
            PlayerPrefs.SetString(wonCircuitsKey, wonCircuits);

            rewardText.text = $"You won ${reward} for this race! Total money: ${currentMoney} and you leveled up your reputation unlocking a new circuit.";
        }
        else
        {
            rewardText.text = $"You won ${reward} for this race! Total money: ${currentMoney}";
        }

        // Guardar los cambios en PlayerPrefs
        PlayerPrefs.Save();
    }

    private void PauseGame()
    {
        Time.timeScale = 0f; // Pausa el juego
        playerHUD.SetActive(false); // Oculta el HUD del jugador
        pausePanel.SetActive(true); // Muestra el panel de pausa
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f; // Reactiva el juego
        pausePanel.SetActive(false); // Oculta el panel de pausa
        playerHUD.SetActive(true); // Muestra el HUD del jugador
    }

    public void RestartRace()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recargar la escena actual
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
