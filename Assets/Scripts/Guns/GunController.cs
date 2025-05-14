using UnityEngine;

public abstract class GunController : GunBase
{
    public abstract void Perform();
    public abstract void ShootProjectile(Vector2 direction);
    public abstract int AmmoCostPerShot { get; }
}
