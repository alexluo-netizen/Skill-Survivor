using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject normalEnemyPrefab;
    [SerializeField] private GameObject fastEnemyPrefab;
    [SerializeField] private GameObject tankEnemyPrefab;
    [SerializeField] private Transform player;

    [Header("Spawn Settings")]
    [SerializeField] private float startSpawnInterval = 1.5f;
    [SerializeField] private float minimumSpawnInterval = 0.7f;
    [SerializeField] private float spawnDistance = 8f;
    [SerializeField] private int maximumEnemies = 40;

    [Header("Difficulty Settings")]
    [SerializeField] private float difficultyRampDuration = 180f;
    [SerializeField] private float endHealthMultiplier = 2f;
    [SerializeField] private float endSpeedMultiplier = 1.2f;

    [Header("Enemy Type Settings")]
    [SerializeField] private float fastEnemyUnlockTime = 30f;
    [SerializeField] private float tankEnemyUnlockTime = 75f;
    [SerializeField] private float fastEnemyChance = 0.3f;
    [SerializeField] private float tankEnemyChance = 0.2f;

    private float spawnTimer;
    private float elapsedTime;

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        spawnTimer += Time.deltaTime;

        float progress = Mathf.Clamp01(
            elapsedTime / Mathf.Max(1f, difficultyRampDuration)
        );

        float currentSpawnInterval = Mathf.Lerp(
            startSpawnInterval,
            minimumSpawnInterval,
            progress
        );

        if (spawnTimer < currentSpawnInterval)
            return;

        spawnTimer = 0f;

        int activeEnemyCount =
            FindObjectsByType<EnemyHealth>(
                FindObjectsSortMode.None
            ).Length;

        if (activeEnemyCount >= maximumEnemies)
            return;

        SpawnEnemy(progress);
    }

    private void SpawnEnemy(float progress)
    {
        Vector2 direction =
            Random.insideUnitCircle.normalized;

        if (direction == Vector2.zero)
        {
            direction = Vector2.right;
        }

        Vector2 spawnPosition =
            (Vector2)player.position
            + direction * spawnDistance;

        GameObject selectedPrefab = ChooseEnemyPrefab();

        GameObject enemy = Instantiate(
            selectedPrefab,
            spawnPosition,
            Quaternion.identity
        );

        float healthMultiplier = Mathf.Lerp(
            1f,
            endHealthMultiplier,
            progress
        );

        float speedMultiplier = Mathf.Lerp(
            1f,
            endSpeedMultiplier,
            progress
        );

        if (enemy.TryGetComponent(
                out EnemyFollow enemyFollow))
        {
            enemyFollow.Initialize(
                player,
                speedMultiplier
            );
        }

        if (enemy.TryGetComponent(
                out EnemyHealth enemyHealth))
        {
            enemyHealth.ApplyDifficulty(
                healthMultiplier
            );
        }
    }

    private GameObject ChooseEnemyPrefab()
    {
        float currentFastChance =
            elapsedTime >= fastEnemyUnlockTime
                ? fastEnemyChance
                : 0f;

        float currentTankChance =
            elapsedTime >= tankEnemyUnlockTime
                ? tankEnemyChance
                : 0f;

        float randomValue = Random.value;

        if (randomValue < currentTankChance)
        {
            return tankEnemyPrefab;
        }

        if (randomValue <
            currentTankChance + currentFastChance)
        {
            return fastEnemyPrefab;
        }

        return normalEnemyPrefab;
    }
}