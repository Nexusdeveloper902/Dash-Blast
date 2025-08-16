using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGun", menuName = "Weapons/Gun")]
public class GunData : ScriptableObject
{
    [Header("Stats")]
    public float fireRate; //Done
    public float bulletSpeed; //Done
    public int bulletsPerShot; //Done
    public float spreadAngle; //Done
    public float bulletDamage; //Done
    public float reloadTime; //Done
    public int maxAmmo; //Done
    public float bulletLifeTime; //Done
    
    
    [Header("References")]
    public GameObject bulletPrefab; //Done
}
