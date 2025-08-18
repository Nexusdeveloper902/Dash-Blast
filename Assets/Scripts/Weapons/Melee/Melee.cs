using UnityEngine;

public abstract class Melee : Weapon
{
    // typed reference to MeleeData (weaponData should point to a MeleeData asset in inspector)
    protected MeleeData meleeData => weaponData as MeleeData;

    protected virtual void Awake()
    {
        if (weaponData == null)
        {
            Debug.LogError($"WeaponData not assigned on {name}.");
        }

        if (meleeData == null)
        {
            Debug.LogError($"WeaponData assigned to {name} is not a MeleeData. Assign a MeleeData asset or change the component.");
        }
    }

    // Melee-specific classes may override TryAttack if they need special checks, otherwise use base TryAttack.
    // Concrete melee implementations implement Attack().
    protected abstract override void Attack();
}