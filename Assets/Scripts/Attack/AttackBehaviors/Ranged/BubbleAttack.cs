using UnityEngine;

public class BubbleAttack : RangedAttack
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
