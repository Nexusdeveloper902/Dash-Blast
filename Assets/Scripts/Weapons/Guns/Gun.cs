using System.Collections;
using UnityEngine;

public abstract class Gun : Weapon
{
    // Typed reference to GunData (weaponData should point to a GunData asset in inspector)
    protected GunData gunData => weaponData as GunData;

    [SerializeField] protected Transform firePoint;

    protected int currentAmmo;
    protected bool isReloading;

    protected virtual void Awake()
    {
        if (weaponData == null)
        {
            Debug.LogError($"WeaponData not assigned on {name}.");
        }

        if (gunData == null)
        {
            Debug.LogError($"WeaponData assigned to {name} is not a GunData. Assign a GunData asset or change the component.");
        }

        currentAmmo = gunData != null ? gunData.MaxAmmo : 0;
    }

    // Gun-specific TryAttack: adds ammo + reload checks
    public override void TryAttack()
    {
        if (isReloading) return;

        // If still on cooldown, skip
        if (attackCooldown > 0f) return;

        if (gunData == null)
        {
            // fallback to base behaviour if no gunData
            base.TryAttack();
            return;
        }

        if (currentAmmo <= 0)
        {
            // auto-start reload if no ammo
            StartCoroutine(Reload());
            return;
        }

        // If player explicitly presses R, reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
            return;
        }

        // OK to attack
        Attack();
        attackCooldown = weaponData.AttackRate;
    }

    // Derived guns implement their own attack behaviour
    protected abstract override void Attack();

    protected virtual IEnumerator Reload()
    {
        if (isReloading || gunData == null) yield break;

        isReloading = true;
        // optionally: play reload animation/sound here
        yield return new WaitForSeconds(gunData.ReloadTime);
        currentAmmo = gunData.MaxAmmo;
        isReloading = false;
    }
}