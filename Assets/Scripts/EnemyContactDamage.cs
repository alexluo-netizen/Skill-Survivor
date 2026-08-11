using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float damageInterval = 1f;

    private PlayerHealth playerInContact;
    private float damageTimer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(
                out PlayerHealth playerHealth))
        {
            return;
        }

        playerInContact = playerHealth;
        damageTimer = 0f;

        playerInContact.TakeDamage(damage);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (playerInContact == null)
            return;

        damageTimer += Time.fixedDeltaTime;

        if (damageTimer >= damageInterval)
        {
            playerInContact.TakeDamage(damage);
            damageTimer = 0f;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerHealth>(out _))
        {
            playerInContact = null;
            damageTimer = 0f;
        }
    }
}