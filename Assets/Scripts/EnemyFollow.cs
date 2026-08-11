using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction =
            ((Vector2)target.position - rb.position).normalized;

        rb.linearVelocity = direction * moveSpeed;
    }

    public void Initialize(Transform newTarget,float speedMultiplier)
    {
        target = newTarget;

        moveSpeed = Mathf.Max(
            0.1f,
            moveSpeed * speedMultiplier
        );
    }
}