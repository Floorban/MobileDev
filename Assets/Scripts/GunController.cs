using UnityEngine;

public abstract class GunController : GunBase
{
    public abstract void Initialize();
    public abstract void OnTouchBegin(Vector2 screenPos);
    public abstract void OnTouchDrag(Vector2 screenPos);
    public abstract void OnTouchEnd(Vector2 screenPos);
    public abstract void Fire(Vector2 direction, int consumedAmmo = 1);
}
