using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameSession gameSession;
    [SerializeField] private PlayerExperience playerExperience;

    [Header("Result UI")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultTitleText;
    [SerializeField] private TMP_Text statsText;
    [SerializeField] private Button restartButton;

    [Header("Selection")]
    [SerializeField] private float selectedScale = 1.08f;

    private bool resultShown;
    private Vector3 restartNormalScale;

    private void Awake()
    {
        restartNormalScale =
            restartButton.transform.localScale;

        resultPanel.SetActive(false);
    }

    private void OnEnable()
    {
        gameSession.GameEnded += ShowResult;
    }

    private void OnDisable()
    {
        if (gameSession != null)
        {
            gameSession.GameEnded -= ShowResult;
        }
    }

    private void Update()
    {
        if (!resultShown ||
            Keyboard.current == null)
        {
            return;
        }

        bool restartPressed =
            Keyboard.current.enterKey.wasPressedThisFrame ||
            Keyboard.current.numpadEnterKey.wasPressedThisFrame ||
            Keyboard.current.spaceKey.wasPressedThisFrame;

        if (restartPressed)
        {
            RestartGame();
        }
    }

    private void ShowResult(GameResult result)
    {
        resultShown = true;

        resultTitleText.text =
            result == GameResult.Victory
                ? "VICTORY!"
                : "GAME OVER";

        int totalSeconds =
            Mathf.FloorToInt(gameSession.ElapsedTime);

        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        statsText.text =
            "<mspace=0.6em>" +
            $"SURVIVAL TIME  {minutes:00}:{seconds:00}\n" +
            $"LEVEL          {playerExperience.Level}\n" +
            $"NORMAL KILLS   {gameSession.NormalKills}\n" +
            $"FAST KILLS     {gameSession.FastKills}\n" +
            $"TANK KILLS     {gameSession.TankKills}\n" +
            $"TOTAL KILLS    {gameSession.TotalKills}" +
            "</mspace>";

        resultPanel.SetActive(true);

        restartButton.transform.localScale =
            restartNormalScale * selectedScale;

        restartButton.Select();
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        Scene currentScene =
            SceneManager.GetActiveScene();

        SceneManager.LoadScene(
            currentScene.buildIndex
        );
    }
}