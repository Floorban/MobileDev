using UnityEngine;

public class AttackBase : MonoBehaviour
{
    public GameObject weaponPrefab;
    [HideInInspector] public GameObject spawnedWeapon;
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
        if (spawnedWeapon == null) return false;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            spawnedWeapon.transform.position + centerOffset, detectionRadius, enemyLayer);
        return hits.Length > 0;
    }
}
