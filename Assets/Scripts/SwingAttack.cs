using DG.Tweening;
using UnityEngine;
public class SwingAttack : MonoBehaviour
{
    private SwingMeleeWeapon stats;
    private bool isSwinging = false;

    public void Setup(SwingMeleeWeapon data)
    {
        stats = data;
    }

    public void DoSwing()
    {
        if (isSwinging) return;

        isSwinging = true;

        float startAngle = -stats.swingAngle / 2;
        float endAngle = stats.swingAngle / 2;
        float prepAngle = -stats.windupAngle;

        transform.localRotation = Quaternion.Euler(0, 0, 0);

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalRotate(new Vector3(0, 0, prepAngle), stats.windupDuration))
            .Append(transform.DOLocalRotate(new Vector3(0, 0, endAngle), stats.swingDuration).SetEase(Ease.OutCubic))
            .AppendCallback(DealDamage)
            .Append(transform.DOLocalRotate(Vector3.zero, stats.returnDuration))
            .OnComplete(() => isSwinging = false);
    }

    private void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position + stats.centerOffset, stats.detectionRadius, stats.enemyLayer);
        foreach (var hit in hits)
        {
            var dmg = hit.GetComponent<IDamageable>();
            if (dmg != null) dmg.TakeDamage(stats.damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (stats == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + stats.centerOffset, stats.detectionRadius);
    }
}
