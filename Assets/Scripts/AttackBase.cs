using UnityEngine;
using System.Collections.Generic;
public class AttackBase : ScriptableObject
{
    public GameObject weaponPrefab;
    public List<GameObject> spawnedWeapons = new();
    public int damage = 10;
    public float detectionRadius = 2f;
    public LayerMask enemyLayer;
    public bool hasActivated;
    public float speed = 10f;
    public float lifeTime = 1f;
    public float cooldown = 1f;
    public Vector3 centerOffset;

    public bool EnemyInRange()
    {
        if (spawnedWeapons == null || spawnedWeapons.Count == 0) return false;

        foreach (var weapon in spawnedWeapons)
        {
            if (!weapon) continue;
            Collider2D[] hits = Physics2D.OverlapCircleAll(
                weapon.transform.position + centerOffset, detectionRadius, enemyLayer);
            if (hits.Length > 0) return true;
        }

        return false;
    }
}
