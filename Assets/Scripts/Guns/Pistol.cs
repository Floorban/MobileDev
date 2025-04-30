  using UnityEngine;

public class Pistol : GunController
{
    public override void Initialize() {
        canShoot = true;
        Setup();
    }
    public override void OnTouchBegin(Vector2 screenPos) {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        aimDir = inputAimDIr * (worldPos - (Vector2)transform.position).normalized;
    }
    public override void OnTouchDrag(Vector2 screenPos) {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        aimDir = inputAimDIr * (worldPos - (Vector2)transform.position).normalized;
    }
    public override void OnTouchEnd(Vector2 screenPos) {
        Fire(aimDir, 1);
    }
    public override void Fire(Vector2 direction, int consumedAmmo) {
        if (!TryFire(consumedAmmo)) return;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * bulletSpeed;
        Destroy(bullet, 1.5f);
    }
}
