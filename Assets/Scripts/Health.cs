using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Health : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    [SerializeField] private Color colorFlash;
    
    private SpriteRenderer spriteRenderer;
    private Color previousColor;

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        previousColor = spriteRenderer.color;
        colorFlash.a = 255;
    }

    public void TakeDamage(float damage)
    {
        currentHealth  -= damage;
        StartCoroutine(Flash());
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Flash()
    {
        spriteRenderer.color = colorFlash;
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = previousColor;
    }
}
