using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stats", menuName = "Enemies/Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("General Stats")]
    public string enemyName;
    public float maxHealth = 100f;
    public float moveSpeed = 3f;
    public float damage = 10f;

    [Header("Combat")]
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;

    [Header("Rewards")]
    public int xpReward = 50;
}
