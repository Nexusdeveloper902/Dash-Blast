using UnityEngine;

public class BaseEnemy : Enemy
{
    protected override void AttackPlayer()
    {
        if (player != null)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(stats.damage);
            }
        }
    }
}