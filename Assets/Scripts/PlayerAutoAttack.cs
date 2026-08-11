using UnityEngine;

public class PlayerAutoAttack : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackInterval = 0.5f;
    [SerializeField] private int projectileDamage = 2;

    [Header("Multishot")]
    [SerializeField] private int projectileCount = 1;
    [SerializeField] private float angleBetweenProjectiles = 15f;

    private float attackTimer;

    private void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackInterval)
        {
            AttackNearestEnemy();
            attackTimer = 0f;
        }
    }

    private void AttackNearestEnemy()
    {
        EnemyHealth[] enemies =
            FindObjectsByType<EnemyHealth>(
                FindObjectsSortMode.None
            );

        if (enemies.Length == 0)
            return;

        Transform nearestEnemy = null;
        float nearestDistanceSquared = float.MaxValue;

        foreach (EnemyHealth enemy in enemies)
        {
            float distanceSquared =
                ((Vector2)enemy.transform.position -
                 (Vector2)transform.position).sqrMagnitude;

            if (distanceSquared < nearestDistanceSquared)
            {
                nearestDistanceSquared = distanceSquared;
                nearestEnemy = enemy.transform;
            }
        }

        Vector2 centerDirection =
            ((Vector2)nearestEnemy.position -
             (Vector2)transform.position).normalized;

        FireProjectiles(centerDirection);
    }

    private void FireProjectiles(Vector2 centerDirection)
    {
        float totalSpread =
            angleBetweenProjectiles * (projectileCount - 1);

        float startingAngle = -totalSpread / 2f;

        for (int i = 0; i < projectileCount; i++)
        {
            float currentAngle =
                startingAngle + angleBetweenProjectiles * i;

            Vector3 rotatedDirection =
                Quaternion.Euler(
                    0f,
                    0f,
                    currentAngle
                ) * (Vector3)centerDirection;

            GameObject projectile = Instantiate(
                projectilePrefab,
                transform.position,
                Quaternion.identity
            );

            projectile.GetComponent<Projectile>().Initialize(
                (Vector2)rotatedDirection,
                projectileDamage
            );
        }
    }

    public void UpgradeDamage()
    {
        projectileDamage += 1;
        Debug.Log($"Projectile Damage: {projectileDamage}");
    }

    public void UpgradeAttackSpeed()
    {
        attackInterval = Mathf.Max(
            0.1f,
            attackInterval * 0.75f
        );

        Debug.Log($"Attack Interval: {attackInterval}");
    }

    public void UpgradeProjectileCount()
    {
        projectileCount = Mathf.Min(
            projectileCount + 2,
            7
        );

        Debug.Log($"Projectile Count: {projectileCount}");
    }
}