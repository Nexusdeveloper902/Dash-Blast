using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth  -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
