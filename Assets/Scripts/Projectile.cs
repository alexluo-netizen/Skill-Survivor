using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 3f;

    private Rigidbody2D rb;
    private Vector2 direction;

    // 当前子弹还能额外穿透几个敌人
    private int remainingPierces;

    // 记录这颗子弹已经伤害过的敌人
    private readonly HashSet<EnemyHealth> hitEnemies = new HashSet<EnemyHealth>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = direction * speed;
    }

    public void Initialize(
        Vector2 newDirection,
        int newDamage,
        int newPiercing)
    {
        direction = newDirection.normalized;
        damage = newDamage;
        remainingPierces = Mathf.Max(0, newPiercing);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
    EnemyHealth enemyHealth =
        other.GetComponentInParent<EnemyHealth>();

    if (enemyHealth == null)
        return;

    // 已经打中过这个敌人，就不重复造成伤害
    if (!hitEnemies.Add(enemyHealth))
        return;

    enemyHealth.TakeDamage(damage);

    if (remainingPierces > 0)
    {
        remainingPierces--;
        return;
    }

    Destroy(gameObject);
    }
}