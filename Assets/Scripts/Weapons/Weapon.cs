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

        // generic input handling — derived classes can override TryAttack to add checks (ammo, reloading, etc.)
        if (Input.GetButton("Fire1") && attackCooldown <= 0f)
        {
            TryAttack();
            attackCooldown = weaponData.AttackRate;
        }
    }

    // Called when the player requests an attack. Default: just call Attack().
    // Gun overrides this to check ammo and trigger reloads.
    protected virtual void TryAttack()
    {
        Attack();
    }

    // The concrete attack implementation (shoot, swing, etc.)
    protected abstract void Attack();
}