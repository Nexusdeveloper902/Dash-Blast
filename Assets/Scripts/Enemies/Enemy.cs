using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected EnemyStats stats; 
    protected Health healthComponent;

    protected float currentHealth;
    protected Transform player;

    // Attack control
    [SerializeField] protected float attackRange = 2f;   // How far enemy can attack
    [SerializeField] protected float attackCooldown = 1f;
    protected float attackTimer = 0f;

    protected virtual void Start()
    {
        healthComponent = GetComponent<Health>();
        healthComponent.maxHealth = stats.maxHealth;
        currentHealth = stats.maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        if (player == null) return;

        attackTimer -= Time.deltaTime;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer > attackRange)
        {
            MoveTowardsPlayer(); // too far -> keep chasing
        }
        else
        {
            StopMoving(); // close enough -> stop and attack
            TryAttack();
        }
    }

    protected virtual void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * stats.moveSpeed * Time.deltaTime;
    }

    protected virtual void StopMoving()
    {
        // If using Rigidbody2D
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
    }

    protected void TryAttack()
    {
        if (attackTimer <= 0f)
        {
            AttackPlayer();
            attackTimer = attackCooldown;
        }
    }

    protected abstract void AttackPlayer();

    public virtual void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{stats.enemyName} died and gave {stats.xpReward} XP");
        Destroy(gameObject);
    }
}
