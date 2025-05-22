using DG.Tweening;
using UnityEngine;
public class SwingAttack : AttackBehavior
{
    public SwingMeleeWeapon _stats;
    public void Setup(SwingMeleeWeapon data)
    {
        _stats = data;
        stats = data;
    }
    public override void Perform()
    {
        if (!isPerforming && canPerform && EnemyInRange())
        {
            DoSwing();
        }
    }
    public void DoSwing()
    {
        if (isPerforming || !canPerform) return;

        isPerforming = true;
        canPerform = false;

        float startAngle = -_stats.swingAngle / 2;
        float endAngle = _stats.swingAngle / 2;
        float prepAngle = -_stats.windupAngle;

        transform.localRotation = Quaternion.Euler(0, 0, 0);

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalRotate(new Vector3(0, 0, prepAngle), _stats.windupDuration))
            .Append(transform.DOLocalRotate(new Vector3(0, 0, endAngle), _stats.swingDuration).SetEase(Ease.OutCubic))
            .AppendCallback(DealDamage)
            .Append(transform.DOLocalRotate(Vector3.zero, _stats.returnDuration))
            .OnComplete(() => {
                isPerforming = false;
                seq.Kill();
            });
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
