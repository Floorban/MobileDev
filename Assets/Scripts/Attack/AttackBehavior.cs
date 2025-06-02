using UnityEngine;

public abstract class AttackBehavior : MonoBehaviour
{
    public WeaponStats stats;
    public bool isPerforming = false;
    public bool canPerform = false;
    protected Transform target;

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
    protected void AimAtEnemy()
    {
        if (target)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }
    public abstract void Perform();

}
