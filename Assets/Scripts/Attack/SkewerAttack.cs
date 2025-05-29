using System.Collections;
using UnityEngine;

public class SkewerAttack : RangedAttack
{
    public override void Fire(Vector2 dir)
    {
        if (isPerforming) return;
        isPerforming = true;
        canPerform = false;
        StartCoroutine(DelayShoot(dir));
    }
    private IEnumerator DelayShoot(Vector2 dir)
    {
        for (int i = 0; i < 3; i++)
        {
            InstantiateProjectile(dir);
            yield return new WaitForSeconds(stats.cooldown / 10f);
        }
        isPerforming = false;
    }
}
