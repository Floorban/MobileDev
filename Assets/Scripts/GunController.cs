using UnityEngine;

public abstract class GunController : GunBase
{
    public abstract void Initialize(Rigidbody2D rb);
    public abstract void OnTouchBegin(Vector2 screenPos);
    public abstract void OnTouchDrag(Vector2 screenPos);
    public abstract void OnTouchEnd(Vector2 screenPos);
    public abstract void Fire(Vector2 direction);
}
