using UnityEngine;

public class IcecreamAttack : RangedAttack
{
    public int pelletCount = 3;
    public float spreadAngle = 30f;
    public override void Fire(Transform firePoint, Vector2 dir)
    {
        if (isPerforming) return;

        isPerforming = true;
        canPerform = false;

        float angleStep = spreadAngle / (pelletCount - 1);
        float startAngle = -spreadAngle / 2f;
        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        for (int i = 0; i < pelletCount; i++)
        {
            float angleOffset = startAngle + i * angleStep;
            float totalAngle = baseAngle + angleOffset;

            Vector2 pelletDir = new Vector2(
                Mathf.Cos(totalAngle * Mathf.Deg2Rad),
                Mathf.Sin(totalAngle * Mathf.Deg2Rad)
            ).normalized;

            GameObject proj = Instantiate(_stats.projectilePrefab, firePoint.position, Quaternion.identity);
            proj.AddComponent<AttackComponent>().damageAmount = 1;

            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            rb.linearVelocity = pelletDir * _stats.speed;

            Destroy(proj, _stats.lifeTime);
        }

        RecoilEffect();
    }
}
