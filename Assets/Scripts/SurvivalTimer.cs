using TMPro;
using UnityEngine;

public class SurvivalTimer : MonoBehaviour
{
    [SerializeField] private float gameDuration = 180f;
    [SerializeField] private TMP_Text timerText;

    private float remainingTime;
    private bool gameFinished;

    private void Awake()
    {
        remainingTime = gameDuration;
        UpdateTimerText();
    }

    private void Update()
    {
        if (gameFinished)
            return;

        if (GameSession.Instance != null &&
            !GameSession.Instance.IsRunning)
        {
            return;
        }

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
        int totalSeconds =
            Mathf.CeilToInt(remainingTime);

        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text =
            $"{minutes:00}:{seconds:00}";
    }

    private void WinGame()
    {
        if (gameFinished)
            return;

        gameFinished = true;

        GameSession.Instance?.TryEndGame(
            GameResult.Victory
        );
    }
}