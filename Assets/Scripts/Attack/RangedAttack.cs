using DG.Tweening;
using UnityEngine;

public class RangedAttack : AttackBehavior
{
    public RangedWeapon _stats;
    [SerializeField] Transform firePoint;
    public void Setup(RangedWeapon data)
    {
        _stats = data;
        stats = data;
        InvokeRepeating(nameof(CheckAndSwing), _stats.cooldown, _stats.cooldown);
    }
    private void CheckAndSwing()
    {
        if (EnemyInRange() && !isPerforming)
        {
            _stats.Fire(firePoint, Vector2.zero);
        }
    }

    private void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + _stats.centerOffset, _stats.detectionRadius, _stats.enemyLayer);
        foreach (var hit in hits)
        {
            var dmg = hit.GetComponent<IDamageable>();
            if (dmg != null) dmg.TakeDamage(_stats.damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (_stats == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _stats.centerOffset, _stats.detectionRadius);
    }
}
