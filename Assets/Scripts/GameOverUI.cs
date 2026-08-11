using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameObject gameOverPanel;

    private bool gameOverShown;

    private void Awake()
    {
        gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (gameOverShown)
            return;

        if (playerHealth.CurrentHealth <= 0)
        {
            gameOverShown = true;
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}