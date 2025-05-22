using UnityEngine;
using System.Collections.Generic;
public abstract class AttackStats : ScriptableObject
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
    public abstract void Activate(WeaponManager p);

}
