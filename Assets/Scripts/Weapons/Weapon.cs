using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponData weaponData;

    // general attack cooldown (seconds between attacks)
    protected float attackCooldown;

    protected virtual void Update()
    {
        if (weaponData == null) return;

        attackCooldown -= Time.deltaTime;
    }

    // Called when the player requests an attack. Default: just call Attack().
    // Gun overrides this to check ammo and trigger reloads.
    public virtual void TryAttack()
    {
        if (attackCooldown <= 0f)
        {
            Attack();
            attackCooldown = weaponData.AttackRate;
        }
    }

    // The concrete attack implementation (shoot, swing, etc.)
    protected abstract void Attack();
}