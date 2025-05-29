using UnityEngine;

public class SkewerAttack : RangedAttack
{
    public override void Fire(Vector2 dir)
    {
        if (isPerforming) return;
        isPerforming = true;
        canPerform = false;
        InstantiateProjectile(dir);
        RecoilEffect();
    }
}
