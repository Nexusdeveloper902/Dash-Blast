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

    protected virtual void Start()
    {
        healthComponent = GetComponent<Health>();
        healthComponent.maxHealth = stats.maxHealth;
        currentHealth = stats.maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        MoveTowardsPlayer();
    }

    protected virtual void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * stats.moveSpeed * Time.deltaTime;
    }

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
        // Example: give XP to player
        Debug.Log($"{stats.enemyName} died and gave {stats.xpReward} XP");
        Destroy(gameObject);
    }

    protected abstract void Attack(Collider2D other);
}