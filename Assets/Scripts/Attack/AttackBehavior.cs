using UnityEngine;

public abstract class AttackBehavior : MonoBehaviour
{
    public WeaponStats stats;
    public bool isPerforming = false;
    public bool canPerform = false;
    public virtual void Update()
    {
        Perform();
    }
    public virtual bool EnemyInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
        transform.position + stats.centerOffset,
        stats.detectionRadius,
        stats.enemyLayer
        );

        if (hits.Length > 0)
            return true;

        return false;
    }

    public abstract void Perform();

}
