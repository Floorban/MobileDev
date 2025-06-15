using DG.Tweening;
using UnityEngine;
public abstract class SwingAttack : AttackBehavior
{
    public SwingMeleeWeapon _stats;
    public void Setup(SwingMeleeWeapon data)
    {
        _stats = data;
        stats = data;
    }
    public override void Update() {
        base.Update();
        if (!isPerforming)
            AimAtEnemy();
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

    public abstract void DealDamage();

    void OnDrawGizmosSelected()
    {
        if (_stats == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _stats.centerOffset, _stats.detectionRadius);
    }


}
