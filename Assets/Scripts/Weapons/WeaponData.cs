using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon")]
public class WeaponData : ScriptableObject
{
    [Header("General")]
    [SerializeField] private float attackRate = 0.2f; // seconds between attacks
    [SerializeField] private float damage = 10f;
    [SerializeField] private float range = 1f; // for melee or ray length for ranged
    [SerializeField] private string weaponName = "New Weapon";

    public float AttackRate => attackRate;
    public float Damage => damage;
    public float Range => range;
    public string WeaponName => weaponName;
}