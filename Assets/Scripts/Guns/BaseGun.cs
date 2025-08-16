using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : Gun
{
    protected override void Shoot()
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
}
