using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : Enemy
{
    protected override void Attack()
    {
        Debug.Log($"{stats.enemyName} slashes player for {stats.damage} damage!");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Attack();
    }
}
