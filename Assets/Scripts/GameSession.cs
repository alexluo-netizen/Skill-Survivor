using System;
using UnityEngine;

public enum EnemyType
{
    Normal,
    Fast,
    Tank
}

public enum GameResult
{
    Victory,
    GameOver
}

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    public bool IsRunning { get; private set; } = true;
    public float ElapsedTime { get; private set; }

    public int NormalKills { get; private set; }
    public int FastKills { get; private set; }
    public int TankKills { get; private set; }

    public int TotalKills =>
        NormalKills + FastKills + TankKills;

    public event Action<GameResult> GameEnded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (IsRunning)
        {
            ElapsedTime += Time.deltaTime;
        }
    }

    public void RegisterKill(EnemyType enemyType)
    {
        if (!IsRunning)
            return;

        switch (enemyType)
        {
            case EnemyType.Normal:
                NormalKills++;
                break;

            case EnemyType.Fast:
                FastKills++;
                break;

            case EnemyType.Tank:
                TankKills++;
                break;
        }
    }

    public bool TryEndGame(GameResult result)
    {
        if (!IsRunning)
            return false;

        IsRunning = false;

        if (result == GameResult.Victory)
        {
            GameAudio.Instance?.PlayVictory();
        }
        else
        {
            GameAudio.Instance?.PlayGameOver();
        }

        Time.timeScale = 0f;
        GameEnded?.Invoke(result);

        return true;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}