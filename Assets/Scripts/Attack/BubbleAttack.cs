using UnityEngine;

public class BubbleAttack : RangedAttack
{
    public override void Fire(Transform target, Vector2 dir)
    {
        if (isPerforming) return;

        isPerforming = true;
        canPerform = false;

        GameObject proj = Instantiate(_stats.projectilePrefab, target.position, Quaternion.identity);
        proj.AddComponent<AttackComponent>().damageAmount = 1;
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        rb.linearVelocity = dir * _stats.speed;
        Destroy(proj, _stats.lifeTime);

        RecoilEffect();
    }
}
