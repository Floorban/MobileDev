using UnityEngine;

public class Rifle : GunController
{
    [Header("Rifle Settings")]
    public float fireRate = 10f;
    private bool isFiring = false;
    private float fireCooldown;
    public float fireSpeed = 1f;
    public override int AmmoCostPerShot => 1;
    public override FireMode fireMode => FireMode.Auto;

    public override void Initialize() {
        canShoot = true;
        fireCooldown = 0f;
        Setup();
    }
    public override void OnTouchBegin(Vector2 screenPos) {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        aimDir = (worldPos - (Vector2)transform.position).normalized;
        isFiring = true;
    }
    public override void OnTouchDrag(Vector2 screenPos) {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        aimDir = (worldPos - (Vector2)transform.position).normalized;

        if (fireCooldown < 0f && TryFire(AmmoCostPerShot)) {
            ShootProjectile(aimDir);
            fireCooldown = 1 / fireRate;
        }
        fireCooldown -= fireSpeed * Time.deltaTime;
    }
    public override void OnTouchEnd(Vector2 screenPos) {
        isFiring = false;
    }
    public override void ShootProjectile(Vector2 direction) {
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = direction.normalized * bulletSpeed;
        Destroy(bullet, bulletLifetime);
    }
}
