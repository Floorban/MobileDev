using UnityEngine;

public class RangeCheck : MonoBehaviour
{
    public AttackBase stats;
    protected bool isPerforming = false;

    public bool EnemyInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(gameObject.transform.position + stats.centerOffset, stats.detectionRadius, stats.enemyLayer);
        if (hits.Length > 0)
            return true;

        return false;
    }
}
