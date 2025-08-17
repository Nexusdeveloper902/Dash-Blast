using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Weapons/Gun")]
public class GunData : WeaponData
{
    [Header("Gun specific")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private int bulletsPerShot = 1;
    [SerializeField] private float spreadAngle = 0f;
    [SerializeField] private float reloadTime = 1.5f;
    [SerializeField] private int maxAmmo = 10;
    [SerializeField] private float bulletLifeTime = 2f;

    public GameObject BulletPrefab => bulletPrefab;
    public float BulletSpeed => bulletSpeed;
    public int BulletsPerShot => bulletsPerShot;
    public float SpreadAngle => spreadAngle;
    public float ReloadTime => reloadTime;
    public int MaxAmmo => maxAmmo;
    public float BulletLifeTime => bulletLifeTime;
}