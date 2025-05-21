using UnityEngine;

public abstract class AttackBehavior : AttackBase
{
    public abstract void Activate(WeaponManager p);
    public abstract void Deactivate(WeaponManager p);
}
