using UnityEngine;

[CreateAssetMenu(fileName = "NewMelee", menuName = "Weapons/Melee")]
public class MeleeData : WeaponData
{
    [Header("Melee specific")]
    [SerializeField] private float hitRadius = 1f;
    [SerializeField] private float hitAngle = 90f;      // cone angle (degrees)
    [SerializeField] private float knockbackForce = 0f;  // optional
    [SerializeField] private LayerMask hitMask = ~0;     // default: everything

    public float HitRadius => hitRadius;
    public float HitAngle => hitAngle;
    public float KnockbackForce => knockbackForce;
    public LayerMask HitMask => hitMask;
}