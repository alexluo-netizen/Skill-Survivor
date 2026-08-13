using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SurvivalTimer : MonoBehaviour
{
    [SerializeField] private float gameDuration = 20f;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject victoryPanel;

    private float remainingTime;
    private bool gameFinished;

    private void Awake()
    {
        remainingTime = gameDuration;
        victoryPanel.SetActive(false);
    }

    private void Update()
    {
        if (gameFinished)
            return;

        remainingTime = Mathf.Max(
            0f,
            remainingTime - Time.deltaTime
        );

        UpdateTimerText();

        if (remainingTime <= 0f)
        {
            WinGame();
        }
    }

    private void UpdateTimerText()
    {
        int totalSeconds = Mathf.CeilToInt(remainingTime);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void WinGame()
    {
        if (gameFinished)
            return;

        gameFinished = true;

        GameAudio.Instance?.PlayVictory();

        victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}