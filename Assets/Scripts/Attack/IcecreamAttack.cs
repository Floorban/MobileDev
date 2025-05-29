using UnityEngine;

public class IceCreamAttack : RangedAttack
{
    public int pelletCount = 3;
    public float spreadAngle = 30f;
    public override void Fire(Vector2 dir)
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

            InstantiateProjectile(pelletDir);
        }

        RecoilEffect();
    }
}
