using DG.Tweening;
using UnityEngine;

public class RangedAttack : AttackBehavior
{
    public RangedWeapon _stats;
    [SerializeField] Transform firePoint;
    private Transform target;
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
            Fire(firePoint, fireDir);
        }
    }
    public void Fire(Transform target, Vector2 dir)
    {
        if (isPerforming) return;
        
        isPerforming = true;
        canPerform = false;

        GameObject proj = Instantiate(_stats.projectilePrefab, target.position, Quaternion.identity);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir * _stats.speed;
        Destroy(proj, _stats.lifeTime);

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
    private void AimAtEnemy()
    {
        if (target)
        {
            Vector3 dir = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0,0,angle - 90f);
        }
    } 
    private void OnDrawGizmosSelected()
    {
        if (_stats == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _stats.centerOffset, _stats.detectionRadius);
    }
}
