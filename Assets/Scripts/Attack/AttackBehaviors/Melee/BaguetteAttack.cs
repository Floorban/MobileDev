using UnityEngine;

public class BaguetteAttack : SwingAttack
{
    public override void DealDamage() {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + _stats.centerOffset, _stats.detectionRadius, _stats.enemyLayer);
        foreach (var hit in hits) {
            var trb = hit.GetComponent<Rigidbody2D>();
            if (trb != null) {
                Vector2 dir = (hit.transform.position - transform.position).normalized;
                trb.AddForce(dir * _stats.knockbackForce, ForceMode2D.Impulse);
            }
            var dmg = hit.GetComponent<IDamageable>();
            if (dmg != null) {
                dmg.TakeDamage(_stats.damage, 0.1f);
            }
        }
    }
}
