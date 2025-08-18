using UnityEngine;

public class BaseGun : Gun
{
    protected override void Attack()
    {
        if (gunData == null || gunData.BulletPrefab == null || firePoint == null) return;

        // Single bullet (no spread)
        if (gunData.BulletsPerShot <= 1 || gunData.SpreadAngle <= 0f)
        {
            var bullet = Instantiate(gunData.BulletPrefab, firePoint.position, firePoint.rotation);
            var bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.Initialize(firePoint.right, gunData.BulletSpeed, weaponData.Damage, player);
            }
            Destroy(bullet, gunData.BulletLifeTime);
        }
        else
        {
            float spreadStep = gunData.SpreadAngle / (gunData.BulletsPerShot - 1);
            float startAngle = -gunData.SpreadAngle / 2f;

            for (int i = 0; i < gunData.BulletsPerShot; i++)
            {
                float currentAngle = startAngle + spreadStep * i;
                Quaternion rotation = Quaternion.Euler(0f, 0f, firePoint.eulerAngles.z + currentAngle);

                var bullet = Instantiate(gunData.BulletPrefab, firePoint.position, rotation);
                var bulletScript = bullet.GetComponent<Bullet>();
                if (bulletScript != null)
                {
                    bulletScript.Initialize(rotation * Vector3.right, gunData.BulletSpeed, weaponData.Damage, player);
                }
                Destroy(bullet, gunData.BulletLifeTime);
            }
        }

        currentAmmo = Mathf.Max(0, currentAmmo - 1);
    }
}