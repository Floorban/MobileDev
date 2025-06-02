using DG.Tweening;
using UnityEngine;

public abstract class RangedAttack : AttackBehavior
{
    public RangedWeapon _stats;
    [SerializeField] private Transform firePoint;
    private Vector2 fireDir;
    public void Setup(RangedWeapon data)
    {
        _stats = data;
        stats = data;
    }

    public override void Update()
    {
        base.Update();
        if (!isPerforming)
            AimAtEnemy();
    }
    public override void Perform()
    {
        if (EnemyInRange() && !isPerforming && canPerform)
        {
            //Fire(firePoint, fireDir);
            DOVirtual.DelayedCall(0.1f, () => Fire(fireDir));
        }
    }
    public abstract void Fire(Vector2 dir);
    public void InstantiateProjectile(Vector2 dir)
    {
        GameObject proj = Instantiate(_stats.projectilePrefab, firePoint.position, Quaternion.identity);
        proj.AddComponent<AttackComponent>().damageAmount = 1;
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir * _stats.speed;
        Destroy(proj, _stats.lifeTime);
    } 
    public void RecoilEffect()
    {
        float currentZ = transform.rotation.eulerAngles.z;

        Sequence recoilSeq = DOTween.Sequence();
        recoilSeq.Append(transform.DORotate(new Vector3(0, 0, currentZ + _stats.recoilAngle), _stats.recoilDuration)
            .SetEase(Ease.OutQuad))
            .Append(transform.DORotate(new Vector3(0, 0, currentZ), _stats.returnDuration)
            .SetEase(Ease.InQuad))
            .OnComplete(() => isPerforming = false);
    }
    public override bool EnemyInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
        transform.position + _stats.centerOffset,
        _stats.detectionRadius,
        _stats.enemyLayer
        );

        if (hits.Length == 0)
            return false;

        // Find the closest enemy
        Transform closest = null;
        float closestDistance = Mathf.Infinity;
        Vector2 origin = firePoint.position;

        foreach (var hit in hits)
        {
            float distance = Vector2.Distance(origin, hit.transform.position);
            if (distance < closestDistance)
            {
                closest = hit.transform;
                closestDistance = distance;
            }
        }

        if (closest)
        {
            target = closest;
            fireDir = (closest.position - firePoint.position).normalized;
            return true;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if (_stats == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _stats.centerOffset, _stats.detectionRadius);
    }
}
