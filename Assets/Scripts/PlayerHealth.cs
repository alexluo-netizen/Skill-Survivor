using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;

    private bool isDead;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        Time.timeScale = 1f;
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Max(CurrentHealth, 0);

        Debug.Log($"Player Health: {CurrentHealth}/{maxHealth}");

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        Debug.Log("Game Over");
        Time.timeScale = 0f;
    }
}