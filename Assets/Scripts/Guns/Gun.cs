using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected GunData gunData;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected int currentAmmo;
    
    protected bool isReloading;
    protected float fireCooldown;
    
    protected virtual void Start()
    {
        currentAmmo = gunData.maxAmmo;
    }

    protected virtual void Update()
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

    protected abstract void Shoot();
    

    protected virtual IEnumerator Reload()
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
