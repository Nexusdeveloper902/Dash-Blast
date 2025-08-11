using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GunData gunData;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int currentAmmo;
    
    private bool isReloading;
    private float fireCooldown;
    
    private void Start()
    {
        currentAmmo = gunData.maxAmmo;
    }

    private void Update()
    {
        if (isReloading)
        {
            return;
        }
        
        fireCooldown -= Time.deltaTime;

        if (Input.GetButton("Fire1") && fireCooldown <= 0f && currentAmmo > 0 && !isReloading)
        {
            Shoot();
            fireCooldown = gunData.fireRate;
        }

        if (!isReloading && (Input.GetKeyDown(KeyCode.R) || (Input.GetButton("Fire1") && currentAmmo <= 0)))
        {
            StartCoroutine(Reload());
        }
    }

    private void Shoot()
    {
        if (gunData.bulletsPerShot == 1)
        {
            // Single bullet: no spread
            var bullet = Instantiate(gunData.bulletPrefab, firePoint.position, firePoint.rotation);
            var bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.Initialize(firePoint.right, gunData.bulletSpeed, gunData.bulletDamage);
            }
            Destroy(bullet, gunData.bulletLifeTime);
        }
        else
        {
            float spreadStep = gunData.spreadAngle / (gunData.bulletsPerShot - 1);
            float startAngle = -gunData.spreadAngle / 2f;

            for (int i = 0; i < gunData.bulletsPerShot; i++)
            {
                float currentAngle = startAngle + spreadStep * i;
                Quaternion rotation = Quaternion.Euler(0, 0, firePoint.eulerAngles.z + currentAngle);

                var bullet = Instantiate(gunData.bulletPrefab, firePoint.position, rotation);
                var bulletScript = bullet.GetComponent<Bullet>();
                if (bulletScript != null)
                {
                    bulletScript.Initialize(rotation * Vector3.right, gunData.bulletSpeed, gunData.bulletDamage);
                }
                Destroy(bullet, gunData.bulletLifeTime);
            }
        }
        currentAmmo--;
    }

    IEnumerator Reload()
    {
        var reloadTimePerBullet = gunData.reloadTime / gunData.maxAmmo;
        isReloading = true;
        
        int bulletsNeeded = gunData.maxAmmo - currentAmmo;
        
        for (int i = 0; i < bulletsNeeded; i++)
        {
            yield return new WaitForSeconds(reloadTimePerBullet);
            currentAmmo++;
        }
        
        isReloading = false;
    }
}
