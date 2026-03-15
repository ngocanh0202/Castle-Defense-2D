using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class UnitCombatBehaviour : MonoBehaviour
{
    [Header("Unit Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackInterval = 1f;

    [Header("Targeting")]
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private string targetTag = "Player";

    private Rigidbody2D rb;
    private Transform currentTarget;
    private float lastAttackTime;
    private ReceiveDamageBehaviour receiveDamageBehaviour;
    private bool isMoving;

    public Transform CurrentTarget => currentTarget;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        receiveDamageBehaviour = GetComponent<ReceiveDamageBehaviour>();
    }

    void OnEnable()
    {
        lastAttackTime = -attackInterval;
        FindNewTarget();
    }

    void Update()
    {
        if (!receiveDamageBehaviour.IsAlive) return;

        if (currentTarget == null)
        {
            FindNewTarget();
            return;
        }

        float distanceToTarget = Vector2.Distance(transform.position, currentTarget.position);

        if (distanceToTarget <= attackRange)
        {
            StopMoving();
            TryAttack();
        }
        else
        {
            MoveTowardsTarget();
        }
    }

    void FixedUpdate()
    {
        if (!receiveDamageBehaviour.IsAlive) return;
        if (isMoving && currentTarget != null)
        {
            Vector2 direction = (currentTarget.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;

            if (direction != Vector2.zero)
            {
                transform.up = direction;
            }
        }
    }

    private void FindNewTarget()
    {
        Collider2D[] potentialTargets = FindObjectsOfType<Collider2D>();
        Transform closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (Collider2D collider in potentialTargets)
        {
            if (collider.CompareTag(targetTag) && collider.gameObject != gameObject)
            {
                ReceiveDamageBehaviour damageBehaviour = collider.GetComponent<ReceiveDamageBehaviour>();
                if (damageBehaviour != null && damageBehaviour.IsAlive)
                {
                    float distance = Vector2.Distance(transform.position, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestTarget = collider.transform;
                    }
                }
            }
        }

        currentTarget = closestTarget;
    }

    private void MoveTowardsTarget()
    {
        if (currentTarget == null) return;
        isMoving = true;
    }

    private void StopMoving()
    {
        isMoving = false;
        rb.velocity = Vector2.zero;
    }

    private void TryAttack()
    {
        if (Time.time - lastAttackTime >= attackInterval)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    private void Attack()
    {
        if (currentTarget == null) return;

        ReceiveDamageBehaviour targetDamage = currentTarget.GetComponent<ReceiveDamageBehaviour>();
        if (targetDamage != null && targetDamage.IsAlive)
        {
            targetDamage.ReceiveDamage(attackDamage);
        }
    }

    public void SetTarget(Transform target)
    {
        currentTarget = target;
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void SetAttackDamage(float damage)
    {
        attackDamage = damage;
    }

    public void SetAttackInterval(float interval)
    {
        attackInterval = interval;
    }

    public void SetTargetTags(string enemyTag, string targetTag)
    {
        this.enemyTag = enemyTag;
        this.targetTag = targetTag;
    }
}
