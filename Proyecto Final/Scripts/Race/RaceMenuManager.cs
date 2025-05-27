using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RaceMenuManager : MonoBehaviour
{
    [Header("Cameras")]
    public GameObject sceneCamera;
    public GameObject playerCamera;

    [Header("UI")]
    public GameObject playerHUD;
    public GameObject startPanel;
    public Text countdownText; // Texto para mostrar la cuenta regresiva
    public GameObject finishMenuPanel; // Panel de fin de carrera
    public Text standingsText; // Texto para mostrar las posiciones

    [Header("Cars")]
    public GameObject playerCar; // Referencia al coche del jugador
    public GameObject[] aiCars; // Referencias a los coches de la IA

    private PlayerCarController playerController;
    private AICarController[] aiControllers;

    void Start()
    {
        Time.timeScale = 0f; // Pausa el juego al inicio
        playerController = playerCar.GetComponent<PlayerCarController>();
        aiControllers = new AICarController[aiCars.Length];

        // Deshabilitar control de coches al inicio
        DisableCarControl(playerCar);
        for (int i = 0; i < aiCars.Length; i++)
        {
            aiControllers[i] = aiCars[i].GetComponent<AICarController>();
            DisableCarControl(aiCars[i]);
        }
    }

    public void StartRace()
    {
        startPanel.SetActive(false); // Oculta el panel de inicio
        sceneCamera.SetActive(false); // Desactiva la c�mara de la escena
        playerCamera.SetActive(true); // Activa la c�mara del jugador
        countdownText.gameObject.SetActive(true); // Activa el texto de cuenta regresiva

        StartCoroutine(StartCountdown()); // Inicia la cuenta regresiva
    }

    private IEnumerator StartCountdown()
    {
        Time.timeScale = 1f; // Reactiva el juego

        int countdown = 3;
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString(); // Muestra el n�mero actual
            yield return new WaitForSeconds(1f); // Espera un segundo
            countdown--;
        }

        countdownText.text = "�GO!"; // Muestra el mensaje de inicio
        yield return new WaitForSeconds(1f); // Espera un segundo m�s
        countdownText.gameObject.SetActive(false); // Oculta el texto de cuenta regresiva

        playerHUD.SetActive(true); // Activa el HUD del jugador
        RaceEventManager.TriggerCountdownFinished(); // Llama al evento de finalizaci�n de la cuenta regresiva
        EnableCarControl(playerCar); // Habilita el control del coche del jugador
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
        sceneCamera.SetActive(true); // Activa la c�mara de la escena
        playerCamera.SetActive(false); // Desactiva la c�mara del jugador
        playerHUD.SetActive(false); // Desactiva el HUD del jugador

        if (finishMenuPanel == null || standingsText == null) return;

        // Ordenar los corredores por progreso
        System.Array.Sort(racers, (a, b) => b.GetProgress().CompareTo(a.GetProgress()));

        // Construir el texto de posiciones
        string standings = "Race Results:\n";
        for (int i = 0; i < racers.Length; i++)
        {
            standings += $"{i + 1}. {racers[i].name}\n";
        }

        // Mostrar resultados en la UI
        standingsText.text = standings;

        // Activar el men� de fin de carrera
        finishMenuPanel.SetActive(true);
    }
}
