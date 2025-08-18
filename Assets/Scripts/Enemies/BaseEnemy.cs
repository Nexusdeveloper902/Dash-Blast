using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : Enemy
{
    protected override void Attack(Collider2D other)
    {
        if (other.GetComponent<Health>() != null)
        {
            other.GetComponent<Health>().TakeDamage(stats.damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Attack(other);
    }
}
