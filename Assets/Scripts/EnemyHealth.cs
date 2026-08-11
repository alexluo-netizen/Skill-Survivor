using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 6;
    [SerializeField] private GameObject experienceGemPrefab;
    [SerializeField] private int experienceReward = 1;

    private int currentHealth;
    private bool isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ApplyDifficulty(float healthMultiplier)
    {
        maxHealth = Mathf.Max(
            1,
            Mathf.RoundToInt(maxHealth * healthMultiplier)
        );

        currentHealth = maxHealth;
    }

    private void Die()
    {
        isDead = true;

        if (experienceGemPrefab != null)
        {
            GameObject gem = Instantiate(
                experienceGemPrefab,
                transform.position,
                Quaternion.identity
            );

            if (gem.TryGetComponent(
                    out ExperienceGem experienceGem))
            {
                experienceGem.Initialize(experienceReward);
            }
        }

        Destroy(gameObject);
    }
}