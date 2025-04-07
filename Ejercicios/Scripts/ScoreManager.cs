using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public Text scoreText;

    private int score1 = 0;
    private int score2 = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void GoalScored(int playerNumber)
    {
        if (playerNumber == 1) score1++;
        else if (playerNumber == 2) score2++;

        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        scoreText.text = $"Player1    {score1} - {score2}    Player2";
    }
}
